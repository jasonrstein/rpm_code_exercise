# rpm_code_exercise


Objective
  
  The objective of this test is for you to create a simple background service implemented as a console application with a scheduled, repeatable task that downloads weekly U.S. fuel pricing from “EIA open api” and saves it to the database. 


Technical Stack 
  
  Required

		.Net framework 4.5 / .Net Core 2.2 or later

		SQL Server, or SQL Express to store the data

		For data access choose ADO.NET or any ORM library

	Optional

		Any third-party libraries you like for scheduling, downloading, and parsing results. 



Detailed Flow Description

	The background service should do the following:

	Download weekly fuel pricing from: http://api.eia.gov/series/?api_key=ec92aacd6947350dcb894062a4ad2d08&series_id=PET.EMD_EPD2D_PTE_NUS_DPG.W

	Parse the data and extract first series section in format [yyyyMMdd, price]

	Do not save data to the database that is older than N days and save it to database in the format [yyyyMMdd, price]

	If record already exists in database - ignore it, duplicates can be checked by yyyyMMdd (record date)



Requirements:

	Successfully create a background process/console application that follows the work flow is described in the previous section.

	Add two parameters that will allow us to configure the behavior of the service: 

		Task execution delay: Delay between background job executions.

		Days count:  This is the 'N days' stated above.

		Suggestion: These parameters could be passed to the background service by including them on a app.config parameter, or json settings, whatever you prefer.  
