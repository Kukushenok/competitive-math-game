:again
if not exist MARKER.txt goto exit
dotnet test --filter Category=Measurement
goto again
:exit
echo "Stopped!"