SELECT distinct Zones.Name, count(Members.Name) AS Members FROM Members JOIN Teams on Members.TeamId = Teams.Id 
													  JOIN ZoneTeams on ZoneTeams.TeamId = Teams.Id
													  JOIN Zones on Zones.Id = ZoneTeams.ZoneId
													  GROUP BY Zones.Name;