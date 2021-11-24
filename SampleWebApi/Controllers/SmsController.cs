using Dapper;
using SampleWebApi.DataAccess;
using SampleWebApi.Dtos;
using SampleWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SampleWebApi.Results;

namespace SampleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        public SmsController()
        {
        }

        [HttpPost("getsms")]
        public IActionResult GetSmsMessages([FromBody] SmsQueryDto requestDto)
        {
            requestDto.StartTime = requestDto.StartTime ?? DateTime.Now.AddYears(-1);
            requestDto.EndTime = requestDto.EndTime ?? DateTime.Now;
            string sql = string.Empty;
            var parametersDictionary = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(requestDto.GsmNumber))
            {
                sql = "SELECT * FROM SMS WHERE GsmNumber = @GsmNumber AND SentOn BETWEEN @StartTime AND @EndTime";
                parametersDictionary.Add("GsmNumber", requestDto.GsmNumber);
                parametersDictionary.Add("StartTime", requestDto.StartTime);
                parametersDictionary.Add("EndTime", requestDto.EndTime);
            }
            else
            {
                sql = "SELECT * FROM SMS WHERE SentOn BETWEEN @StartTime AND @EndTime";
                parametersDictionary.Add("@StartTime", requestDto.StartTime);
                parametersDictionary.Add("@EndTime", requestDto.EndTime);
            }

            SmsApiResult result = new SmsApiResult();
            using (SqlConnection connection = DbHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    var smses = connection.Query<Sms>(sql, parametersDictionary).ToList();
                    IList<SmsResponseDto> smsList = new List<SmsResponseDto>();
                    foreach (var sms in smses)
                    {
                        SmsResponseDto smsResponseDto = new SmsResponseDto();
                        smsResponseDto.Id = sms.Id;
                        smsResponseDto.GsmNumber = sms.GsmNumber;
                        smsResponseDto.Message = sms.Message;
                        smsResponseDto.SentOn = sms.SentOn;

                        smsList.Add(smsResponseDto);
                    }
                    result.Success = true;
                    result.Result = smsList;
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Error = ex.Message;
                }
                return Ok(result);
            }
        }

        [HttpPost("sendsms")]
        public IActionResult SendSms([FromBody] SendSmsDto requestDto)
        {
            if (requestDto.SentOn == DateTime.MinValue)
                requestDto.SentOn = DateTime.Now;

            var parameters = new { SentOn = requestDto.SentOn, GsmNumber = requestDto.GsmNumber, Message = requestDto.Message };
            var sql = "INSERT INTO SMS (SentOn, GsmNumber, Message) VALUES (@SentOn, @GsmNumber, @Message)";

            SmsApiResult result = new SmsApiResult();

            using (SqlConnection connection = DbHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    var rowsAffected = connection.Execute(sql, parameters);
                    result.Success = rowsAffected > 0;

                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Error = ex.Message;
                }
                return Ok(result);
            }
        }
    }
}
