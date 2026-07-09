[Code]

const
    FirewallStdAppsRegPath = 'SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\AuthorizedApplications\List';
    FirewallDomAppsRegPath = 'SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\AuthorizedApplications\List';
    FirewallStdPortsRegPath = 'SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\GloballyOpenPorts\List';
    FirewallDomPortsRegPath = 'SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\GloballyOpenPorts\List';

var
    FirewallRestrict2Local: boolean;

//----------------------------------------------------------------------------
// OS Version conditions
//----------------------------------------------------------------------------
 
//----------------------------------------------------------------------------
function GetWinMajMin(): integer;
var
    WindowsVersion: TWindowsVersion;
 
begin
    GetWindowsVersionEx (WindowsVersion);
    Result := 100 * WindowsVersion.Major +  WindowsVersion.Minor;
end; 

//----------------------------------------------------------------------------
function IsWindowsXP(): boolean;
begin
    Result := (GetWinMajMin() < 600);
end;

function IsWindowsVista(): boolean;
begin
    Result := (GetWinMajMin() < 601);
end;

//----------------------------------------------------------------------------
function GetFirewallScope (Param: string): string;
begin
    if (IsWindowsXP()) then begin
        if (FirewallRestrict2Local) then
            Result := 'LocalSubNet'
        else
            Result := '*';
    end else if (IsWindowsVista()) then begin
        if (FirewallRestrict2Local) then
            Result := 'SUBNET'
        else
            Result := 'ALL';
    end else begin
        if (FirewallRestrict2Local) then
            Result := 'LOCALSUBNET'
        else
            Result := 'ANY';
    end;
end;
 
//----------------------------------------------------------------------------
procedure ExecFirewallConfiguration (CliCmd: string);
var
    ResultCode: integer;
    Args: string;
 
begin
    Args := CliCmd;
    if not Exec (ExpandConstant ('{sys}\netsh.exe'), Args, '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then begin
        MsgBox ('Execution failed with result ' + Variant(ResultCode) + ': ' + ExpandConstant ('{sys}\netsh.exe') + ' ' + Args,  mbInformation, MB_OK);
    end;
end;

//----------------------------------------------------------------------------
// Firewall exceptions
//----------------------------------------------------------------------------
 
//----------------------------------------------------------------------------
procedure AddFirewallPrgmException (AppPath, RuleName: string);
 
var
    CliCmd: string;
 
begin
  if (IsWindowsXP()) then begin
       RegWriteStringValue (HKEY_LOCAL_MACHINE, FirewallStdAppsRegPath, AppPath, AppPath + ':' + GetFirewallScope('') + ':Enabled:' + RuleName);
       RegWriteStringValue (HKEY_LOCAL_MACHINE, FirewallDomAppsRegPath, AppPath, AppPath + ':' + GetFirewallScope('') + ':Enabled:' + RuleName);
  end else begin
       if (IsWindowsVista()) then
           CliCmd := 'firewall add allowedprogram program="' + AppPath + '" name="' + RuleName + '" mode=ENABLE scope=' + GetFirewallScope('') + ' profile=ALL'
       else
           CliCmd := 'advfirewall firewall add rule name="' + RuleName + '" program="' + AppPath + '" profile=ANY dir=IN action=ALLOW enable=YES remoteip=' + GetFirewallScope('');
       ExecFirewallConfiguration (CliCmd);
  end;
end;
 
//----------------------------------------------------------------------------
procedure RmvFirewallPrgmException (AppPath, RuleName: string);
 
var
    CliCmd: string;
 
begin
    if (IsWindowsXP()) then begin
        RegDeleteValue (HKEY_LOCAL_MACHINE, FirewallStdAppsRegPath, AppPath);
        RegDeleteValue (HKEY_LOCAL_MACHINE, FirewallDomAppsRegPath, AppPath);
    end else begin
        if (IsWindowsVista) then
            CliCmd := 'firewall delete allowedprogram program="' + AppPath + '" profile=ALL'
        else
            CliCmd := 'advfirewall firewall delete rule name="' + RuleName + '" profile=ANY';
        ExecFirewallConfiguration (CliCmd);
    end;
end;

//----------------------------------------------------------------------------
procedure AddFirewallPortException (Protocol, Port, RuleName: string);
 
var
    CliCmd: string;
    Key: string;
 
begin
    if (IsWindowsXP()) then begin
        Key := Port + ':' + Protocol;
        RegWriteStringValue (HKEY_LOCAL_MACHINE, FirewallStdPortsRegPath, Key, Key + ':' + GetFirewallScope('') + ':Enabled:' + RuleName);
        RegWriteStringValue (HKEY_LOCAL_MACHINE, FirewallDomPortsRegPath, Key, Key + ':' + GetFirewallScope('') + ':Enabled:' + RuleName);
    end else begin
        if (IsWindowsVista()) then
            CliCmd := 'firewall add portopening protocol=' + Protocol + ' port=' + Port + ' name="' + RuleName + '" mode=ENABLE scope=' + GetFirewallScope('') + ' profile=ALL'
        else
            CliCmd := 'advfirewall firewall add rule name="' + RuleName + '" protocol=' + Protocol + ' localport=' + Port + ' profile=ANY dir=IN action=ALLOW enable=YES remoteip=' + GetFirewallScope('');
        ExecFirewallConfiguration (CliCmd);
    end;
end;


//----------------------------------------------------------------------------
procedure AddFirewallPortExceptionEx (CompList: TStringList; Protocol, Port, RuleName: string);
 
var
    CompIsSelected: boolean;
    i: integer;
 
begin
    CompIsSelected := False;
    for i := 0 to CompList.Count-1 do begin
        if (IsComponentSelected (CompList[i])) then
            CompIsSelected := True;
    end;
 
    if (CompIsSelected) then begin
      AddFirewallPortException(Protocol,Port,RuleName);
    end;
end;
 
//----------------------------------------------------------------------------
procedure RmvFirewallPortException (Protocol, Port, RuleName: string);
 
var
    CliCmd: string;
    Key: string;
 
begin
    if (IsWindowsXP()) then begin
        Key := Port + ':' + Protocol;
        RegDeleteValue (HKEY_LOCAL_MACHINE, FirewallStdPortsRegPath, Key);
        RegDeleteValue (HKEY_LOCAL_MACHINE, FirewallDomPortsRegPath, Key);
    end else begin
        if (IsWindowsVista()) then
            CliCmd := 'firewall delete portopening protocol=' + Protocol + ' port=' + Port + ' profile=ALL'
        else
            CliCmd := 'advfirewall firewall delete rule name="' + RuleName + '" profile=ANY';
        ExecFirewallConfiguration (CliCmd);
    end;
end;

 
