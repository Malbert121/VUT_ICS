﻿namespace ICS.BL.Models;
public record StudentSubjectDetailModel : ModelBase
{
    public Guid StudentId { get; set; }
    public Guid SubjectId { get; set; }
    public string StudentFirstName { get; set; } = string.Empty;
    public string StudentLastName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public string SubjectAbbreviation { get; set; } = string.Empty;
    public static StudentSubjectDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        StudentId = Guid.Empty,
        SubjectId = Guid.Empty,
        StudentFirstName = string.Empty,
        StudentLastName = string.Empty,
        SubjectName = string.Empty,
        SubjectAbbreviation = string.Empty
    };
}

