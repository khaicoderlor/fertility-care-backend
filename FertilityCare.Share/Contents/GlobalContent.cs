using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fertilitycare.Share.Contents
{
    public class GlobalContent
    {
        public const string RaiseAppointmentEmailTemplate = @"<!DOCTYPE html>
                            <html lang=""vi"">
                            <head>
                                <meta charset=""UTF-8"">
                                <title>Xác nhận đặt lịch</title>
                            </head>
                            <body style=""font-family: Arial, sans-serif; background-color: #f7f7f7; padding: 20px;"">
                                <table style=""max-width: 600px; margin: auto; background: white; border-radius: 10px; padding: 20px;"">
                                    <tr>
                                        <td style=""text-align: center;"">
                                            <h2 style=""color: #4CAF50;"">Xác nhận đặt lịch điều trị thành công</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>Chào <strong>{{PatientName}}</strong>,</p>
                                            <p>Bạn đã đặt lịch điều trị thành công tại trung tâm IVF.</p>
                                            <p><strong>Thông tin lịch hẹn:</strong></p>
                                            <ul style=""line-height: 1.6;"">
                                                <li><strong>Ngày điều trị: </strong>{{TreatmentDate}}</li>
                                                <li><strong>Giờ hẹn:</strong>{{Time}}</li>
                                                <li><strong>Bác sĩ phụ trách:</strong>{{DoctorName}}</li>
                                            </ul>
                                            <p>Nếu bạn có bất kỳ câu hỏi hoặc cần thay đổi lịch, vui lòng liên hệ với chúng tôi qua hotline: <strong>1900 1234</strong>.</p>
                                            <p>Chúng tôi rất hân hạnh được đồng hành cùng bạn trong hành trình điều trị.</p>
                                            <br />
                                            <p>Trân trọng,<br />Trung tâm hỗ trợ sinh sản (IVF Center)</p>
                                        </td>
                                    </tr>
                                </table>
                            </body>
                            </html>";
    }
}
