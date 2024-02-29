using ICSProj;

using (var context = new SchoolContext())
{
    var student = new StudentEntity { firstName = "Jan", lastName = "Novák", fotoURL = "http://example.com/jan.jpg" };
    context.Students.Add(student);
    context.SaveChanges();
}

Console.WriteLine("Student added successfully!");

using (var context = new SchoolContext())
{
    var students = context.Students.ToList();
    foreach (var student in students)
    {
        Console.WriteLine($"Student: {student.firstName} {student.lastName}, URL: {student.fotoURL}");
    }
}

using (var context = new SchoolContext())
{
    var student = context.Students.FirstOrDefault(s => s.firstName == "Jan" && s.lastName == "Novák");
    if (student != null)
    {
        student.fotoURL = "http://example.com/new-jan.jpg";
        context.SaveChanges();
        Console.WriteLine("Student updated successfully!");
    }
}