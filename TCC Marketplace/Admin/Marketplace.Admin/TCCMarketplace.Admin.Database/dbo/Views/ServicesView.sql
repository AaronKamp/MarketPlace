
CREATE VIEW [dbo].[ServicesView]
	AS 

	
with serviceZipStates(serId, StateId, StateName, CountryId, CountryName
) as
	(
	select distinct * from (
		select ss.ServiceId, scf.StateId, st2.State_Name, st2.CountryId, c2.Country_Name
		from ServiceSCF ss
		join SCF scf on ss.SCFId = scf.Id
		join State st2 on st2.Id = scf.StateId
		join Country c2 on c2.Id = st2.CountryId
		)x
	)
	
select distinct
ser.Id,
ser.Tilte,
ser.ShortDescription,
ser.[InAppPurchaseId], 
[IconImage],[StartDate],
[EndDate],[URL],
ser.IsActive,
st.[Description] as ServiceType,
ser.ZipCodes,
States = STUFF((SELECT distinct ', ' + szs.StateName
                                    FROM serviceZipStates szs                                   
									where szs.serId = ser.Id
                                      FOR XML Path('')),1,1,''),
Countries = STUFF((SELECT distinct ', ' + szc.CountryName
                                    FROM serviceZipStates szc                                   
									where szc.serId = ser.Id
                                      FOR XML Path('')),1,1,''),
SCFCodes = STUFF((SELECT distinct ', ' + scfCodes.SCFCode
                                    FROM (select sscf.ServiceId serId, scf.DisplayText SCFCode from ServiceSCF sscf                                   
										join SCF scf on scf.Id = sscf.SCFId
                                     )scfCodes where scfCodes.serId = ser.Id FOR XML Path('')),1,1,''),
Thermostats = STUFF((SELECT distinct ', ' + serProd.ProductName
                                    FROM (select sp.ServiceId serId, p.Name ProductName from ServiceProduct sp                                   
										join Product p on p.Id = sp.ProductId
                                     )serProd where serProd.serId = ser.Id FOR XML Path('')),1,1,''),
ser.UpdatedDate
from Service ser
join ServiceType st on st.Id = ser.ServiceTypeId
left join serviceZipStates szs on szs.serId = ser.Id