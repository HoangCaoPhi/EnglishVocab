+ Chấp nhận, chứa, trung chuyển dữ liệu binary - messages.
+ Producing là gửi message đi. Chương trình gửi message đi gọi là Producer.
+ Hàng đợi chỉ bị ràng buộc bởi giới hạn bộ nhớ và ổ đĩa của máy chủ, 
về cơ bản nó là một bộ đệm tin nhắn lớn.
+ Nhiều producers  có thể gửi tin nhắn đến một hàng đợi và nhiều consumers có thể cố gắng nhận dữ liệu từ một hàng đợi.
+ Consuming là nhận, chương trình nhận gọi là Consumer.
+ Sử dụng giao thức mở AMQP 0-9-1.
+ message content là một byte array.
+ Các message Publish theo async nên có hàm EventingBasicConsumer xử lý gọi lại.
+ round-robin: phân bổ đều cho các consumer.
+ message sẽ được publish cho các consumer. không quan tâm consumer có xử lý xong hay chưa, không giữ message ở queue.
+ Ví dụ có 3 messeage gửi đến một consumer. nhưng nó xử lý quá lâu cái đầu rồi die. Vậy
có thể có nguy cơ mất cả 3 message chưa xử lý?
=>  message acknowledgments
+ một ack là consumer nói với Rabit rằng một message cụ thể đã được nhận và xử lý,rabit có thể xóa nó.
nếu không nó sẽ gửi cho consumer khác nếu woker die.
+ trường hợp rabitmq die:
	+ rabit không cho redefine lại hàng đợi đã có.
	+ set durable = true
	+ var properties = channel.CreateBasicProperties();
      properties.Persistent = true;
	+ thực chất lưu ở cache chứ không phải disk.
	+ tìm hiểu thêm publisher confirm.
+ giảm tải cho các worker nhận tin nhắn
	+	BasicQos với prefetchCount = 1 => Rabit không gửi nhiều tin nhắn cho một woker cùng một lúc, mà nó sẽ ưu tiên gửi cho các woker không bận.
+ Publish/Subscribe
	+	Message Model: producer không gửi bất kì message nào trực tiếp đến một queue. Thay vào đó nó gửi đến Exchange.
		+ exchange type:
			+	direct, topic, headers và fanout
	+ fanout: nhận được bao nhiêu message gửi hết cho all queue nó biết.
	+ relationship giữa exchange và queue gọi là binding.
	+ direct : 1 messgae đi đến đúng key sao cho binding key khớp với router key.
	+ 
