using FakeQuotaSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FakeQuotaSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmplQuotaController : ControllerBase
    {
        private static readonly List<EmplQuota> _fakeData = new List<EmplQuota>
        {
            new EmplQuota
            {
                Id = 1,
                RegionId = "BJ",
                QuotaSeqNo = 1,
                Year = 2025,
                ApplicationType = "北京",
                DayAmount = 365,
                HourAmount = 365 * 24,
                QuotaDayAmount = 365,
                QuotaHourAmount = 365 * 24 * 60,
                Remarks = "北京年度额度"
            },
            {
                Id = 2,
                RegionId = "BJ",
                QuotaSeqNo = 2,
                Year = 2025,
                ApplicationType = "深圳",
                DayAmount = 365,
                HourAmount = 365 * 24,
                QuotaDayAmount = 365,
                QuotaHourAmount = 365 * 24 * 60,
                Remarks = "深圳年度额度"
            },
            {
                Id = 3,
                RegionId = "BJ",
                QuotaSeqNo = 3,
                Year = 2025,
                ApplicationType = "北京护理",
                DayAmount = 365,
                HourAmount = 365 * 24,
                QuotaDayAmount = 365,
                QuotaHourAmount = 365 * 24 * 60,
                Remarks = "北京护理年度额度"
            },
            {
                Id = 4,
                RegionId = "SZ",
                QuotaSeqNo = 1,
                Year = 2025,
                ApplicationType = "深圳",
                DayAmount = 365,
                HourAmount = 365 * 24,
                QuotaDayAmount = 365,
                QuotaHourAmount = 365 * 24 * 60,
                Remarks = "深圳年度额度"
            },
            {
                Id = 5,
                RegionId = "SZ",
                QuotaSeqNo = 2,
                Year = 2025,
                ApplicationType = "深圳护理",
                DayAmount = 365,
                HourAmount = 365 * 24,
                QuotaDayAmount = 365,
                QuotaHourAmount = 365 * 24 * 60,
                Remarks = "深圳护理年度额度"
            }
        };

        [HttpGet]
        [Route("all")]
        public ActionResult<IEnumerable<EmplQuota>> GetAll()
        {
            return Ok(_fakeData);
        }

        [HttpGet]
        [Route("{id:long}")]
        public ActionResult<EmplQuota> GetById(long id)
        {
            var quota = _fakeData.FirstOrDefault(q => q.Id == id);
            if (quota == null)
            {
                return NotFound();
            }
            return Ok(quota);
        }

        [HttpPost]
        [Route("create")]
        public ActionResult<EmplQuota> Create([FromBody] EmplQuota quota)
        {
            quota.Id = _fakeData.Count + 1;
            _fakeData.Add(quota);
            return CreatedAtAction("/", quota);
        }

        [HttpPut]
        [Route("{id:long}")]
        public ActionResult<EmplQuota> Update(long id, [FromBody] EmplQuota quota)
        {
            var existing = _fakeData.FirstOrDefault(q => q.Id == id);
            if (existing == null)
            {
                return NotFound();
            }
            existing.RegionId = quota.RegionId;
            existing.QuotaSeqNo = quota.QuotaSeqNo;
            existing.Year = quota.Year;
            existing.ApplicationType = quota.ApplicationType;
            existing.DayAmount = quota.DayAmount;
            existing.HourAmount = quota.HourAmount;
            existing.QuotaDayAmount = quota.QuotaDayAmount;
            existing.QuotaHourAmount = quota.QuotaHourAmount;
            existing.Remarks = quota.Remarks;
            return NoContent();
        }

        [HttpDelete]
        [Route("{id:long}")]
        public ActionResult Delete(long id)
        {
            var existing = _fakeData.FirstOrDefault(q => q.Id == id);
            if (existing == null)
            {
                return NotFound();
            }
            _fakeData.Remove(existing);
            return NoContent();
        }
    }
}
