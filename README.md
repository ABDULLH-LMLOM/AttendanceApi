# 📋 AttendanceApi

نظام **REST API** لإدارة حضور الطلاب في الامتحانات، مبني بـ **ASP.NET Core Web API** و **Entity Framework Core**، ومتكامل مع جهاز **ESP32** لالتقاط بيانات الحضور (RFID / بصمة) بشكل مباشر عند دخول الطالب.

🔗 **Live API (Swagger):** [attendapi.runasp.net/swagger](https://attendapi.runasp.net/swagger/index.html)

---

## 📖 نظرة عامة

المشروع بيسمح بـ:
- تسجيل حضور ودخول الطلاب لامتحان معين لحظة وصولهم، عن طريق استقبال `Student IDs` وتحديث وقت الدخول تلقائيًا.
- إرسال الـ `Student ID` لجهاز **ESP32** (عبر `HttpClient`) لعرضه أو معالجته على الجهاز نفسه (real-time device integration).
- عرض بيانات كل امتحان (تاريخه، عدد الطلاب الكلي، وعدد الحاضرين).
- عرض سجل تفصيلي (Record) لكل طالب في امتحان معين (وقت الدخول والخروج).
- تعديل وقت امتحان معين ليبدأ "الآن" (مفيد للتشغيل اللحظي/الاختبار الميداني).

---

## 🛠️ التقنيات المستخدمة

| التقنية | الاستخدام |
|---|---|
| **ASP.NET Core 8 Web API** | بناء الـ REST endpoints |
| **Entity Framework Core 8** | ORM للتعامل مع قاعدة البيانات |
| **SQL Server** | قاعدة البيانات |
| **Swashbuckle (Swagger)** | توثيق واختبار الـ API |
| **IHttpClientFactory** | التواصل مع جهاز ESP32 عبر HTTP |
| **WebSockets** | مفعّلة على مستوى الـ pipeline لدعم الاتصال اللحظي |

---

## 📂 هيكل المشروع

```
AttendanceApi/
├── Controllers/
│   ├── AttendanceController.cs      # تسجيل حضور الطلاب + إرسال البيانات لـ ESP32
│   ├── ExamsController.cs           # إدارة وعرض بيانات الامتحانات وسجلات الحضور
│   └── AuthenticationsController.cs # تسجيل الدخول (قيد التطوير)
├── Data/
│   └── AppDbContext.cs              # DbContext وتعريف العلاقات بين الجداول
├── Models/
│   ├── Student.cs
│   ├── Exam.cs
│   ├── Sector.cs
│   ├── StudentsExams.cs             # جدول ربط (Many-to-Many) بين الطالب والامتحان
│   ├── User.cs
│   └── Dto/ExamDto.cs
├── Service/
│   └── AttendanceService.cs         # منطق إرسال بيانات الطالب لجهاز ESP32
├── Migrations/                      # EF Core Migrations
└── Program.cs                       # نقطة تشغيل المشروع وإعداد الـ pipeline
```

---

## 🔌 الـ Endpoints

### Attendance

| Method | Route | الوصف |
|---|---|---|
| `PUT` | `/api/Attendance/Taking_Attendance` | استقبال قائمة `Student IDs`، تسجيل وقت دخولهم للامتحان الجاري حاليًا، وإرسال كل ID لجهاز ESP32 |

### Exams

| Method | Route | الوصف |
|---|---|---|
| `GET` | `/api/Exams/GetExam?date=` | جلب كل الامتحانات في تاريخ معين مع عدد الطلاب الكلي وعدد الحاضرين |
| `GET` | `/api/Exams/ViewRecord?ExamId=` | عرض سجل الطلاب (الاسم، القسم، وقت الدخول والخروج) لامتحان معين |
| `PUT` | `/api/Exams/EditExamTime_now?id=` | ضبط بداية ونهاية الامتحان لتبدأ من الوقت الحالي (مدتها 3 ساعات) |

### Authentication

| Method | Route | الوصف |
|---|---|---|
| `POST` | `/api/Authentications/login` | تسجيل الدخول (endpoint موجود، لسه من غير منطق تحقق فعلي) |

> 📌 التوثيق التفاعلي الكامل لكل الـ endpoints (Request/Response models) متاح على [Swagger UI](https://attendapi.runasp.net/swagger/index.html).

---

## 🗄️ نموذج قاعدة البيانات (ERD مختصر)

- **Sector** ← يحتوي على أكثر من **Exam**
- **Exam** ← مرتبط بـ **Student** عن طريق جدول **StudentsExams** (Many-to-Many) ويحمل `EntryTime` و `ExitTime`
- **Student** ← له اسم وقسم (`Department`)
- **User** ← بيانات مستخدم النظام (مستخدَمة للـ Authentication مستقبلًا)

---

## ⚙️ التشغيل محليًا

### المتطلبات
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (Local أو أي instance متاح)

### الخطوات

```bash
# 1. Clone المشروع
git clone https://github.com/ABDULLH-LMLOM/AttendanceApi.git
cd AttendanceApi

# 2. اضبط Connection String في appsettings.json
# "ConnectionStrings": { "PublicServer": "your-connection-string" }

# 3. طبّق الـ Migrations لإنشاء قاعدة البيانات
dotnet ef database update

# 4. شغّل المشروع
dotnet run
```

بعد التشغيل، هيتم تحويلك تلقائيًا لصفحة Swagger على:
`https://localhost:{port}/swagger/index.html`

> ⚠️ **ملاحظة:** جزء الـ **ESP32 integration** بيستهدف IP ثابت على الشبكة المحلية (`AttendanceService`)، فلازم تتأكد إن الجهاز على نفس الشبكة أو تعدّل الـ `BaseAddress` في `Program.cs` حسب بيئتك.

---

## 🚀 تحت التطوير (Roadmap)

- [ ] تفعيل **JWT Authentication** بشكل كامل على `AuthenticationsController`
- [ ] إضافة **Repository Pattern** و **Service Layer** بشكل أشمل
- [ ] إضافة Validation على مستوى الـ DTOs
- [ ] إضافة Unit Tests

---

## 👤 المطور

**Abdullah**
- GitHub: [@ABDULLH-LMLOM](https://github.com/ABDULLH-LMLOM)
