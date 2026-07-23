using IrregularVerbsApp.Models;
using IrregularVerbsApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=verbs.db"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Создаём базу данных при старте
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// API ENDPOINTS
// Получить все глаголы (для справочника)
app.MapGet("/api/verbs", async (AppDbContext db) =>
{
    return await db.Verbs.ToListAsync();
});

// Получить случайный вопрос (Множественный выбор)
app.MapGet("/api/quiz/multiple-choice", async (AppDbContext db) =>
{
    var allVerbs = await db.Verbs.ToListAsync();
    if (allVerbs.Count < 4) return Results.BadRequest("Недостаточно глаголов в базе");

    // Выбираем правильный глагол
    var target = allVerbs[Random.Shared.Next(allVerbs.Count)];

    // Подбираем 3 случайных неправильных варианта (дистракторы)
    var wrongOptions = allVerbs
        .Where(v => v.Id != target.Id)
        .OrderBy(_ => Random.Shared.Next())
        .Take(3)
        .Select(v => $"{v.PastSimple} / {v.PastParticiple}")
        .ToList();

    // Добавляем правильный ответ и перемешиваем
    var correctAnswer = $"{target.PastSimple} / {target.PastParticiple}";
    var options = wrongOptions.Append(correctAnswer).OrderBy(_ => Random.Shared.Next()).ToList();

    return Results.Ok(new
    {
        QuestionId = target.Id,
        Infinitive = target.Infinitive,
        Translation = target.Translation,
        Options = options
    });
});

// Получить вопрос для ручного ввода
app.MapGet("/api/quiz/fill-in", async (AppDbContext db) =>
{
    var count = await db.Verbs.CountAsync();
    var randomVerb = await db.Verbs.Skip(Random.Shared.Next(count)).FirstOrDefaultAsync();

    if (randomVerb == null) return Results.NotFound();

    return Results.Ok(new
    {
        QuestionId = randomVerb.Id,
        Infinitive = randomVerb.Infinitive,
        Translation = randomVerb.Translation
    });
});

// Проверить ответ пользователя (Ручной ввод)
app.MapPost("/api/quiz/check", async (CheckAnswerDto dto, AppDbContext db) =>
{
    var verb = await db.Verbs.FindAsync(dto.QuestionId);
    if (verb == null) return Results.NotFound("Глагол не найден");

    // Приводим к нижнему регистру и убираем лишние пробелы для точного сравнения
    bool isV2Correct = string.Equals(verb.PastSimple.Trim(), dto.PastSimpleAnswer?.Trim(), StringComparison.OrdinalIgnoreCase);
    bool isV3Correct = string.Equals(verb.PastParticiple.Trim(), dto.PastParticipleAnswer?.Trim(), StringComparison.OrdinalIgnoreCase);

    bool isSuccess = isV2Correct && isV3Correct;

    return Results.Ok(new
    {
        IsCorrect = isSuccess,
        CorrectV2 = verb.PastSimple,
        CorrectV3 = verb.PastParticiple
    });
});

app.Run();

// DTO (Data Transfer Object) для получения ответа с фронтенда
public record CheckAnswerDto(int QuestionId, string PastSimpleAnswer, string PastParticipleAnswer);