-- FortBorders
-- Author: Geekob
-- DateCreated: 5/12/2017 5:28:54 PM
--------------------------------------------------------------

local forts = {};
local fortImprovementId = -1;

--functions hooked on inGame events--

function OnCityAddedToMap( ownerPlayerID:number, cityID:number )
	local pCity = Players[ownerPlayerID]:GetCities():FindID( cityID );
	print ("City added to map: "..pCity:GetName());
	for fort,fortPlots in pairs(forts) do
		if(fort:GetOwner() == ownerPlayerID) then
			local tempPlotTable = {};
			local isFortInsideCityTerritory = (Map.GetPlotDistance(fort:GetX(),fort:GetY(),pCity:GetX(),pCity:GetY()) <= 3);
			print("Fort distance: "..Map.GetPlotDistance(fort:GetX(),fort:GetY(),pCity:GetX(),pCity:GetY()));
			for _,plot in ipairs(fortPlots) do
				local tempPlot = AddPlotIfNotOwned(plot,ownerPlayerID,isFortInsideCityTerritory,true);
				if tempPlot ~= nil then table.insert(tempPlotTable,tempPlot); end
			end
      
			for _,i in pairs(fortPlots) do
				table.remove(fortPlots);
			 end
			forts[fort] = tempPlotTable;
		end
	end
	print ("Forts checked");
end

function OnImprovementAddedToMap(locX, locY, eImprovementType, eOwner)
	local improvementData:table = GameInfo.Improvements[eImprovementType];
	local plot = Map.GetPlot(locX, locY);
	
	if(eOwner ~= -1) and (not plot:IsOwned()) and (eOwner ~= 63)  and ((improvementData.ImprovementType == "IMPROVEMENT_FORT") or (improvementData.ImprovementType == "IMPROVEMENT_ROMAN_FORT")) then
		fortImprovementId = eImprovementType;
		print ("Fort added to map");
		local fortPlots = {};
		local tempPlot = nil;
		tempPlot = AddPlotIfNotOwned(plot,eOwner,false,false);
		local isFortInsideCityTerritory = true;

		if tempPlot ~= nil then 
		  table.insert(fortPlots,tempPlot); 
		  isFortInsideCityTerritory = false;
		  print ("Fort is not inside city territory");
		end

		local nearestCity = GetNearestCity(plot:GetX(),plot:GetY(),eOwner);
		
		for i=0,5,1 do --for every direction
			local plotOnDirection = Map.GetAdjacentPlot(plot:GetX(), plot:GetY(), i);
			tempPlot = AddPlotIfNotOwned(plotOnDirection,eOwner,isFortInsideCityTerritory,false);
			if tempPlot ~= nil then table.insert(fortPlots,tempPlot); end
		end
		forts[plot] = fortPlots;
		print ("Adding fort`s plots ended");
	end
end

function OnImprovementRemovedFromMap( locX :number, locY :number, eOwner :number )
	local plot = Map.GetPlot(locX, locY);
	local wasFort = false;

	for fort,fortPlots in pairs(forts) do
		if fort:GetIndex() == plot:GetIndex() then
			wasFort = true;
		end
	end

	if wasFort then
		OnImprovementChanged(locX, locY, fortImprovementId, eOwner, nil, 1, 0);
	end
end

function OnImprovementChanged(locationX, locationY, improvementType, improvementOwner, resource, isPillaged, isWorked)
	local improvementData:table = GameInfo.Improvements[improvementType];

	if (isPillaged == 0) and ((improvementData.ImprovementType == "IMPROVEMENT_FORT") or (improvementData.ImprovementType == "IMPROVEMENT_ROMAN_FORT")) then
		OnImprovementAddedToMap(locationX, locationY, improvementType, improvementOwner);
		return;
	end

	local plot = Map.GetPlot(locationX, locationY);
	local adjacentPlots = GetPlotsNotNearCity(plot);

	if (adjacentPlots ~= nil) and ((improvementData.ImprovementType == "IMPROVEMENT_FORT") or (improvementData.ImprovementType == "IMPROVEMENT_ROMAN_FORT")) and (isPillaged) then
		
		for i=0,5,1 do --for every direction
			local plotOnDirection = Map.GetAdjacentPlot(locationX, locationY, i);
			local oppositeDirection = math.fmod(i + 3, 6);

			--dont affect plot that is connected to other outpost
			if (not IsAdjacentToOtherOutpost(plotOnDirection:GetX(), plotOnDirection:GetY(), oppositeDirection, improvementOwner) ) then
				if(plotOnDirection:GetImprovementType() ~= -1) then ImprovementBuilder.SetImprovementPillaged(plotOnDirection, true); end
				plotOnDirection:SetOwner(-1);
			end
		end
		plot:SetOwner(-1);
		table.remove(forts,plot);
	end

end

--Helper Functions--

function IsAdjacentToOtherOutpost(targetPlotX, targetPlotY, outpostDirection, owner)
	
	for i=0,5,1 do
		if(outpostDirection ~= i) then
			if ( IsOutpost(targetPlotX,targetPlotY,owner,i) ) then return true; end
		end
	end

	return false;
end

function IsOutpost(locX, locY, owner, direction)
	local plot = Map.GetAdjacentPlot(locX, locY, direction);

	if(plot:GetOwner() ~= owner) then return false; end

	local improvementType = plot:GetImprovementType();

	if(improvementType == -1) then return false; end

	local improvementData:table = GameInfo.Improvements[improvementType];

	if((improvementData.ImprovementType == "IMPROVEMENT_FORT") or (improvementData.ImprovementType == "IMPROVEMENT_ROMAN_FORT")) then
		return true;
	end
	return false;
end

function AddPlotIfNotOwned(plot:table,eOwner:number,isFortInsideCityTerritory:boolean,changeCityOwner:boolean) --return plot, if its not owned by city
	if (plot == nil) or (eOwner == -1) then return nil; end
	if (plot:GetOwner() == -1) or (changeCityOwner and plot:GetOwner() == eOwner) then 
		local nearestCity = GetNearestCity(plot:GetX(),plot:GetY(),eOwner);

		--if plot is close to city, it will be owned by the city
		if (Map.GetPlotDistance(plot:GetX(),plot:GetY(),nearestCity:GetX(),nearestCity:GetY()) <= 3) or isFortInsideCityTerritory then
			print("Owner ID: "..eOwner..", Location: "..plot:GetX().." "..plot:GetY()..", Nearest city: "..nearestCity:GetName());
			plot:SetOwner(-1);--switching tile ownership bugs out, so first reset owner of plot
			WorldBuilder.CityManager():SetPlotOwner(plot, nearestCity);
			print("Fort`s plot added to city");
		else 
			plot:SetOwner(eOwner);
			print("Fort`s plot added"); 
			return plot;
		end	 
	end
	return nil;
end

function GetNearestCity(locX, locY, iOwner)
	local minDistance = 1000;
	local nearestCity = nil;
	local players = Players[iOwner];
	local cities = players:GetCities();

	if cities ~= nil then
		for _,city in cities:Members() do

			local distance = Map.GetPlotDistance(locX,locY,city:GetX(),city:GetY());

			if distance < minDistance then 
				minDistance = distance;
				nearestCity = city; 
			end
		end
	end

	return nearestCity;
end

function GetPlotsNotNearCity(iPlot)
  local queue = {};
  local head  = 1;
  local tail  = 1;

  local function push(plot)
    queue[tail] = plot;
    tail = tail + 1;
  end

  local function pop()
    if head == tail then return nil; end

    local plot = queue[head];
    head = head + 1;
    return plot;
  end

  local result = {};
  local i = 0;

  local function add(plot)
    result[i] = plot;
    i = i + 1;
  end

  local function isInResults(plot)
	for j=0, i, 1 do
		if result[j] == plot then return true; end
	end
	return false;
  end

  local currentPlot = iPlot;
  local owner = iPlot:GetOwner();

  if(owner == -1) then return nil; end	

  while currentPlot do
    local isCity = currentPlot:IsCity();
	if(isCity) and (currentPlot:GetOwner() == owner) then return nil; end

	for i=0,5,1 do
		local plotOnDirection = Map.GetAdjacentPlot(currentPlot:GetX(), currentPlot:GetY(), i);
		if(plotOnDirection ~= nil) and (plotOnDirection:GetOwner() == owner) and (not isInResults(plotOnDirection)) then 
			push(plotOnDirection); add(plotOnDirection); 
		end
	end

    currentPlot = pop();
  end

  return result;
end

Events.ImprovementChanged.Add( OnImprovementChanged );
Events.ImprovementAddedToMap.Add(OnImprovementAddedToMap );
Events.CityAddedToMap.Add( OnCityAddedToMap );
Events.ImprovementRemovedFromMap.Add(OnImprovementRemovedFromMap );
