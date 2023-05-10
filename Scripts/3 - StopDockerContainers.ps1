# Create Docker Container if Image exists
$dockerContainers = @("shopping-cart", "special-offers", "loyalty-program");

ForEach ($dockerContainer in $dockerContainers)
{
    Write-Output "-- - - - - - - - - - - - - - - - - - - - - - - -";
    Write-Output "Stopping Docker Container: $($dockerContainer)...";
    docker stop $dockerContainer;
    Write-Output "Docker Container stopped..";
    Write-Output "-- - - - - - - - - - - - - - - - - - - - - - - -";
}