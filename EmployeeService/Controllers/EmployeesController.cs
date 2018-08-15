﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace EmployeeService.Controllers
{
    public class EmployeesController : ApiController
    {
        [HttpGet]
         public IEnumerable<Employee> LoadAllEmployees()
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }
        }
        [HttpGet]
        public HttpResponseMessage LoadSpecificEmployeebyID(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Employee with Id " + id.ToString() + " not found");
                }
            }
        }
        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                try
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                 }
                catch(Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                    
                }
            }
        }
        public HttpResponseMessage Delete(int id)
        {
            try
            {
               
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Employee eith Id = " + id.ToString() + " not found to delete");
                    }
                    else { 
                    entities.Employees.Remove(entity);
                    entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.NotFound, ex);
            }
        }
        public HttpResponseMessage Put(int id, [FromBody] Employee employee)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {

                try
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entities == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee With ID = " + id.ToString() + "is not found to update");
                    }
                    else
                    {
                        entity.FirstName = employee.FirstName;
                        entity.Gender = entity.Gender;
                        entity.LastName = employee.LastName;
                        entity.Salary = employee.Salary;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
                catch (Exception ex)
                {

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest , ex );
                }
            }
        }
    }
}
