# DIV-SOUND-backend

## Description
This api is just the backend for storing data into the database and ftp server


## Prerequisite

**Docker:**
- docker
- ftp server with user and password
- database with the provided setup.sql
- [div-sound-socket](https://git.digitalindividuals.com/rvleeuwen/div-sound-socket) (this is not really used in the api but it is part of the backend)


**Native:**
- dotnet
- ftp server with user and password
- database with the provided setup.sql
- [div-sound-socket](https://git.digitalindividuals.com/rvleeuwen/div-sound-socket) (this is not really used in the api but it is part of the backend)




## Installation

perform the following steps:

### Docker

```Bash
docker pull digitalroseuwu/div-sound-backend:latest

docker run -p 8080:80 --env SqlServer="<your-mysql-connectionstring>" --env ftpServer=ftp://<your ip address> --env ftpUsername=<your ftp username> --env ftpPassword=<your ftp password> --env ftpPath=files digitalroseuwu/div-sound-backend
```

the connectionstring can look like this:

"Server=123.456.789.123;port=3306;Database=DIVSOUND;User Id=Root;Password=supersecretpassword"

### Native

#### windows (powershell)
```powershell
git clone https://git.digitalindividuals.com/rvleeuwen/div-sound-backend.git

set-location div-sound-backend
$Env:SqlServer = "<your-mysql-connectionstring>"
$Env:ftpServer = "ftp://ipadress"
$Env:ftpUsername = "username"
$Env:ftpPassword = "password"
$Env:ftpPath = "files"

dotnet start
```

#### linux
```bash
git clone https://git.digitalindividuals.com/rvleeuwen/div-sound-backend.git

cd div-sound-backend

export SqlServer="<your-mysql-connectionstring>"
export ftpServer="ftp://ipadress"
export ftpUsername="username"
export ftpPassword="password"
export ftpPath="files"

dotnet start
```

