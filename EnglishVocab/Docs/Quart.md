+ Để khởi tạo lập lịch: implement ISchedulerFactory.
+ Trigger chỉ được kịch hoạt khi scheduler được khởi động, nếu shutdown hoặc tạm dừng nó sẽ không work.
+ Key interface và class:
	+ IScheduler: cung cấp API tương tác bộ lập lịch.
	+ IJob: interface của thành phần sẽ thực thi khi lập lịch.
	+ IJobDetail: một instance của job.
	+ ITrigger:	thành phần xác định IJob sẽ thực thi khi nào, một job có thể có nhiều trigger.
	+ JobBuilder: dùng build JobDetail.
	+ TriggerBuilder: dùng để build Trigger.
	+ SchedulerBuilder: build Schedule.
+ vòng đời của Scheduler's bắt đầu khi gọi SchedulerFactory và kết thúc khi gọi Shutdown. Thêm xóa job, trigger ở instance này.
+ Job:
	+ IJobDetail được tạo khi một job được thêm vào Schedule.
	+ Jobdetail build các config cho job.
	+ JobDataMap: cung cấp cấu hình thuộc tính và giữ chúng qua những lần thực thi job, cấu hình dữ liệu có sẵn cho job.
	+ Nó chứa các property setting cho một job và JobDataMap (lưu trữ thông tin cho một instance Job class).
	+ 


+ Triggers:
	+ Trigger object để thực thi hoặc fire một job.
	+ Trigger cũng có một JobDataMap  liên quan đến chúng. giúp pass param chỉ định cho một job.
	+ SimpleTrigger: phù hợp với những công việc kích hoạt một lần và mãi mãi.
	+ CronTrigger: phù hợp với những lịch trình.
+ Identities:
	+ Key hoàn chỉnh là kết hợp của Key Job và key Trigger.