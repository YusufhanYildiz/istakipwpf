using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IsTakipWpf.Models;
using IsTakipWpf.Repositories;

namespace IsTakipWpf.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _repository;

        public JobService(IJobRepository repository)
        {
            _repository = repository;
        }

        public async Task<(bool Success, string Message, int JobId)> CreateJobAsync(Job job)
        {
            var (isValid, message) = ValidateJob(job);
            if (!isValid)
            {
                return (false, message, 0);
            }

            try
            {
                var id = await _repository.AddAsync(job);
                return (true, "İş başarıyla oluşturuldu.", id);
            }
            catch (Exception ex)
            {
                return (false, $"İş oluşturulurken bir hata oluştu: {ex.Message}", 0);
            }
        }

        public async Task<bool> DeleteJobAsync(int id)
        {
            return await _repository.SoftDeleteAsync(id);
        }

        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            return await _repository.GetAllAsync(includeDeleted: false);
        }

        public async Task<Job> GetJobAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Job>> SearchJobsAsync(string searchTerm, int? customerId = null, JobStatus? status = null)
        {
            return await _repository.SearchAsync(searchTerm, customerId, status);
        }

        public async Task<(bool Success, string Message)> UpdateJobAsync(Job job)
        {
            var (isValid, message) = ValidateJob(job);
            if (!isValid)
            {
                return (false, message);
            }

            try
            {
                var result = await _repository.UpdateAsync(job);
                return result ? (true, "İş güncellendi.") : (false, "İş bulunamadı veya güncellenemedi.");
            }
            catch (Exception ex)
            {
                return (false, $"İş güncellenirken bir hata oluştu: {ex.Message}");
            }
        }

        public async Task<bool> UpdateJobStatusAsync(int id, JobStatus status)
        {
            var job = await _repository.GetByIdAsync(id);
            if (job == null) return false;

            job.Status = status;
            return await _repository.UpdateAsync(job);
        }

        private (bool IsValid, string Message) ValidateJob(Job job)
        {
            if (job.CustomerId <= 0)
            {
                return (false, "Müşteri seçilmelidir.");
            }

            if (string.IsNullOrWhiteSpace(job.JobTitle))
            {
                return (false, "İş başlığı boş bırakılamaz.");
            }

            if (job.StartDate.HasValue && job.EndDate.HasValue && job.EndDate < job.StartDate)
            {
                return (false, "Bitiş tarihi başlangıç tarihinden önce olamaz.");
            }

            return (true, string.Empty);
        }
    }
}