using API.DTOs.Asset;
using API.DTOs.Department;
using API.DTOs.Designation;
using API.DTOs.Document;
using API.DTOs.Employee;
using API.DTOs.User;
using API.Models.Entities;
using API.Models.Identity;
using AutoMapper;

namespace API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Department
        CreateMap<Department, DepartmentDto>();
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>();

        // Designation
        CreateMap<Designation, DesignationDto>();
        CreateMap<CreateDesignationDto, Designation>();
        CreateMap<UpdateDesignationDto, Designation>();

        // Employee
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.DepartmentName,
                opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.DesignationName,
                opt => opt.MapFrom(src => src.Designation.Name))
            .ForMember(dest => dest.ManagerName,
                opt => opt.MapFrom(src => src.Manager != null ? src.Manager.FullName : null));

        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>();

        // Asset
        CreateMap<Asset, AssetDto>()
                   .ForMember(dest => dest.EmployeeName,
                       opt => opt.MapFrom(src =>
                           src.Employee != null ? src.Employee.FullName : null));
        CreateMap<CreateAssetDto, Asset>();
        CreateMap<UpdateAssetDto, Asset>();

        // Document
        CreateMap<Document, DocumentDto>()
            .ForMember(
                dest => dest.EmployeeName,
                opt => opt.MapFrom(src => src.Employee.FullName)
            );
    }
}