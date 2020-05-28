using System;
using System.Collections.Generic;
using System.Text;

namespace Curso2020.ImportingEmployeeFiles.ExceptionClass
{
	public class HttpResponseException : Exception
	{
		public int Status { get; set; }
		public object Value { get; set; }

		public static HttpResponseException BadRequest(string message) =>
			new HttpResponseException() { Status = 400, Value = message };
		public static HttpResponseException Conflict(string message) =>
			new HttpResponseException() { Status = 409, Value = message };

		public static HttpResponseException InternalServer(string message) =>
			new HttpResponseException() { Status = 500, Value = message };
	}
}
