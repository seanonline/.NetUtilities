<Query Kind="Program">
  <Connection>
    <ID>96d581fb-a809-46df-bb89-f29159ddcdd3</ID>
    <Persist>true</Persist>
    <Server>localhost</Server>
    <Database>Northwind</Database>
  </Connection>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <NuGetReference>DocumentFormat.OpenXml</NuGetReference>
  <NuGetReference>MailKit</NuGetReference>
  <Namespace>DocumentFormat.OpenXml</Namespace>
  <Namespace>DocumentFormat.OpenXml.Packaging</Namespace>
  <Namespace>DocumentFormat.OpenXml.Spreadsheet</Namespace>
  <Namespace>MailKit</Namespace>
  <Namespace>MailKit.Net.Smtp</Namespace>
  <Namespace>MimeKit</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Specialized</Namespace>
  <Namespace>System.Configuration</Namespace>
  <Namespace>System.Data.Common</Namespace>
  <Namespace>System.Dynamic</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <AppConfig>
    <Content>
      <configuration>
        <connectionStrings>
          <add name="UspsConnection" connectionString="data source=usps-cat-mssql.cxnrmqekeqtq.us-east-1.rds.amazonaws.com;initial catalog=USPS_Scheduling;persist security info=True;user id=pass_cat_master;password=MyV0ice1sMyPa$$port;MultipleActiveResultSets=True;App=LINQPad" />
        </connectionStrings>
        <appSettings>
          <add key="fromDate" value="2016-01-01" />
          <add key="toDate" value="2018-01-01" />
          <add key="workingDirectory" value="C:\REPORTS-DO-NOT-TOUCH\Reports" />
          <add key="workingDirectoryNonDaily" value="C:\REPORTS-DO-NOT-TOUCH\Reports-Non-Daily" />
          <add key="workingDirectoryCounts" value="C:\REPORTS-DO-NOT-TOUCH\Counts" />
          <add key="emailTo" value="azaman@deloitte.com; altabor@deloitte.com; tbealer@deloitte.com; willijones@deloitte.com; fsakr@deloitte.com;wperley@deloitte.com" />
          <add key="emailFrom" value="azaman.deloitte@gmail.com" />
        </appSettings>
      </configuration>
    </Content>
  </AppConfig>
</Query>

static string emailFrom = "testdev@gmail.com";


void Main()
{

	//Get orders placed by customers in the system
	List<NotificationList> OdersInSystem = GetOrdersFromSystem();
	

    //Manage the MaxDegreeOfParallelism instead of .NET Managing this. We dont need 500 threads spawning for this.
	var numEmailsToSendAtOneTime =  System.Environment.ProcessorCount * 2;

	
	//numEmailsToSendAtOneTime.Dump();
	var parallelOptions = new ParallelOptions
	{
		MaxDegreeOfParallelism = numEmailsToSendAtOneTime
	};

	Parallel.ForEach (OdersInSystem.Select(r => r.Email).Distinct().ToList(),parallelOptions,email=>{
		try
		{
			var message = constructMessageFromNotification(OdersInSystem, email);
			
			//for Testing
	   //	SendMail("abc@gmail.com",message);		
			
			//For Production
		  //  SendMail(email,message);
			
			email.Dump();
			message.Dump();
		}
		catch(Exception ex) 
		{
		   ex.StackTrace.Dump();
		}		 
	});
	
	"Done".Dump();	
}


public void SendMail(string Email , string msg)
{
	var message = new MimeMessage();
	message.From.Add(new MailboxAddress(emailFrom, emailFrom));	
	
	message.To.Add(new MailboxAddress(Email.Trim(), Email.Trim()));
	message.Subject = string.Format("Daily Appointment Update Reminder - {0}", DateTime.Now.ToShortDateString());
	string emailMessage =msg;

	var builder = new BodyBuilder();
	builder.HtmlBody = emailMessage;
	message.Body = builder.ToMessageBody();


	using (var client = new SmtpClient())
	{
		// For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
		client.ServerCertificateValidationCallback = (s, c, h, e) => true;

		client.Connect("127.0.0.2", 2529, false);

		// Note: since we don't have an OAuth2 token, disable
		// the XOAUTH2 authentication mechanism.
		// client.AuthenticationMechanisms.Remove("XOAUTH2");

		// Note: only needed if the SMTP server requires authentication
//		client.Authenticate("AKIAJM4KYO3A57WQUTGQ", "Ag2NuW8L4MmqR+NViFAR3x4AylyjoJ3++n3hdPiwtyJu");

		client.Send(message);
		client.Disconnect(true);
	}
}

public string constructMessageFromNotification(List<NotificationList> notification1, string Email)
{
	StringBuilder sb = new StringBuilder();
	
	sb.Append("<html><head><meta content='text/html; charset=utf-8' http-equiv='content-type'/></head>");	
	sb.Append("<body bgcolor='#FFFFFF'>");
	
	sb.Append("<b> Good morning,</b><br/><br/>");
	
	sb.Append("Happy Christ Day " +  DateTime.Now.ToString() + "  Hope you have a good day.");

    sb.Append("<br/><br/> Than you for placing order with us<br/><br/>");

	sb.Append("<br/><br/><table>");	

	sb.Append("</table>");
	sb.Append("<br/> Thanks  Sales Team");
	
	sb.Append("</body>");
	sb.Append("</html>");
	
	
	return sb.ToString();
}

// return orders placed by customers in system
public List<NotificationList> GetOrdersFromSystem()
{
	return Orders.Select(row => new NotificationList()
	 {
		  CustomerID =  row.CustomerID,
		  OrderID = row.OrderID,
		  Email = row.Customer.Email
	 }).ToList();
}

#region DTOandPlaceholders

//DTO's and placeholders
public class NotificationList
{
	public int OrderID { get; set; }
	public string CustomerID { get; set; }
	public string Email { get;set;}
}

#endregion