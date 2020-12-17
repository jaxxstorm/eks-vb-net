Imports Pulumi

Module Program

    Sub Main()
        Deployment.RunAsync(Of NginxStack)().Wait()
    End Sub

End Module
