using ICSProj;

using (var context = new SchoolContext())
{
    var student = new Student { firstName = "Jan", lastName = "Novák", fotoURL = "http://example.com/jan.jpg" };
    context.Users.Add(student);
    context.SaveChanges();
}

Console.WriteLine("Student added successfully!");