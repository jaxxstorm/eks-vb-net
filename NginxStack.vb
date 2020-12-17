Imports Pulumi.Kubernetes.Apps.V1
Imports Pulumi.Kubernetes.Core.V1
Imports Pulumi.Kubernetes.Types.Inputs.Apps.V1
Imports Pulumi.Kubernetes.Types.Inputs.Core.V1
Imports Pulumi
Imports Pulumi.Kubernetes.Types.Inputs.Meta.V1


Class NginxStack 
  Inherits Stack

  Public Sub New()
    
    
    
    ' an input map of labels
    Dim labels = New InputMap(Of String) From {{"app", "nginx"}}
    
    ' define the deployment spec
    Dim containerPortArgs = New ContainerPortArgs With { .ContainerPortValue = 80 }
    Dim containerArgs = New ContainerArgs With { 
          .Image = "nginx",
          .Name = "nginx",
          .Ports = containerPortArgs }
    Dim podSpecArgs = New PodSpecArgs With { .Containers = containerArgs }
    Dim template = New PodTemplateSpecArgs With { .Spec = podSpecArgs, .Metadata = New ObjectMetaArgs With { .Labels = labels } }
    Dim labelSelectorArgs = New LabelSelectorArgs With { .MatchLabels = labels }
    Dim deploymentSpecArgs = New DeploymentSpecArgs With { 
          .Template = Template,
          .Replicas = 2,
          .Selector = labelSelectorArgs}
    
    ' create the actual deployment
    Dim deployment = New Pulumi.Kubernetes.Apps.V1.Deployment("nginx", New DeploymentArgs With { .Spec = deploymentSpecArgs })
    
    ' define the service spec
    Dim servicePortArgs = New ServicePortArgs With { .Port = 80, .TargetPort = 80 }
    Dim serviceSpecArgs = New ServiceSpecArgs With { .Ports = servicePortArgs, .Type = "LoadBalancer", .Selector = labels }
    Dim objectMetaArgs = New ObjectMetaArgs With { .Name = "nginx", .Labels = labels }
    
    ' create a service
    Dim svc = New Pulumi.Kubernetes.Core.V1.Service("nginx", New ServiceArgs With { .Metadata = objectMetaArgs, .Spec = serviceSpecArgs } )
    
    Address = svc.Status.Apply(Function(status) If(status.LoadBalancer.Ingress(0).Ip, status.LoadBalancer.Ingress(0).Hostname))
    
    
  End Sub
  
  <Output>
  Public Property Address As Output(Of String)
    

    
    
End Class