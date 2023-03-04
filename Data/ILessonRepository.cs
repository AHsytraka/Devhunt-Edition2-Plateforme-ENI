using Devhunt.Models;

namespace Devhunt.Data;

public interface ILessonRepository
{
    Lesson CreateLesson(Lesson lesson);
}