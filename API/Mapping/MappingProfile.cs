using API.DTOs.Asset;
using API.DTOs.Dashboard.Widgets;
using API.DTOs.Department;
using API.DTOs.Designation;
using API.DTOs.Document;
using API.DTOs.Employee;
using API.DTOs.Permission;
using API.Models.Authorization;
using API.Models.Entities;
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
                opt => opt.MapFrom(src => src.Manager == null ? null : src.Manager.FullName));

        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>();

        // Asset
        CreateMap<Asset, AssetDto>()
            .ForMember(dest => dest.EmployeeName,
                opt => opt.MapFrom(src => src.Employee == null ? null : src.Employee.FullName));

        CreateMap<CreateAssetDto, Asset>();
        CreateMap<UpdateAssetDto, Asset>();

        // Document
        CreateMap<Document, DocumentDto>()
            .ForMember(dest => dest.EmployeeName,
                opt => opt.MapFrom(src => src.Employee.FullName));

        // Permission
        CreateMap<Permission, PermissionDto>();

        CreateMap<Employee, RecentEmployeeDto>()
            .ForMember(d => d.Department,
                o => o.MapFrom(s => s.Department.Name))
            .ForMember(d => d.Designation,
                o => o.MapFrom(s => s.Designation.Name))
            .ForMember(d => d.Status,
                o => o.MapFrom(s => s.Status.ToString()));
    }
}