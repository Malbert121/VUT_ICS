using ICS.BL.Models;
using ICS.DAL.Entities;

namespace ICS.BL.Mappers
{
    public interface IStudentModelMapper : IModelMapper<StudentEntity, StudentListModel, StudentDetailModel>
    {
    }
}