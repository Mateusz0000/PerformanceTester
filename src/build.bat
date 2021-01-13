D:
cd D:\Developing\PerformanceTester\src
rmdir Artifacts /s /q
mkdir Artifacts^
	Artifacts\IIS^;
	
set projectDir=D:\Developing\PerformanceTester\src\PerformanceTester\PerformanceTester
set solutionDir=D:\Developing\PerformanceTester\src\PerformanceTester
	
REM Kompilowanie solucji
dotnet build %solutionDir%\PerformanceTester.sln

REM publikowanie binarek dla iis
dotnet publish %projectdir%\performancetester.csproj -o artifacts\iis -c iis

REM Tworzenie obrazu Dockera
docker stop performance_tester_local
docker image rm perf_tester
docker rm performance_tester_local
docker build -t perf_tester %projectDir%
docker run --name performance_tester_local -d -p 5000:5000 -p 5001:5001