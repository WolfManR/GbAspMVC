using ScannerSpammerDevice_User;
using System.IO;
using System.Text;
using ActualUserOfScanSpamDevice;
using ActualUserOfScanSpamDevice.DataSaveStrategies;
using ActualUserOfScanSpamDevice.ParseHandlers;
using Autofac;
using ScannerSpammerDevice;

//===File generation=========================================================================
const string filePath = "File.txt";

string fileContent = @"Image Title=""Spaghetti"";Content=""h4i3p2kn43on34no34in34ni6j56i4j7j57o45j5l23l3l5j2lj636l34j6"";

Note Title=""Eat"";Body=""Try Eat Something"";

Note Title=""Write a note"";Body=""Custom not description"";

Note Title=""Read all text"";Body=""Read all notes"";

Image Title=""Bread"";Content=""h4i3p328h09igo23hvo9jw9340vijv48303hbrj84jg9vj4iv43b34ib43jb0934jg436l34j6"";

Image Title=""Eggs"";Content=""h4i3p2kn43on34no34in34ni6j56i4j7onbe9wpogjj84j09vkmv32iu4b39jv2ova8ugl4hvkk84gbjleb349jbn349b4j3b8943bier8943bhefuidbhr9pegjl43hj57o45j5l23l3l5j2lj636l34j6"";

";

File.WriteAllText(filePath, fileContent, Encoding.UTF8);

//===Actual program=============================================================================

var container = new ContainerBuilder().RegisterMany(builder =>
{
    builder.RegisterType<ScanSpamDeviceUsingHandler>().As<IScanSpamDeviceHandler>();
    builder.RegisterType<ConsoleLogger>().As<ILogger>().SingleInstance();
    builder.RegisterType<ConsoleSaveStrategy>().As<IDataSaveStrategy>();
    builder.RegisterType<ScanSpamDevice>().As<IScanSpamDevice>();
    builder.RegisterTypes(typeof(ImageParseHandler), typeof(NoteParseHandler)).As<ParseHandler>();
}).Build();


IScanSpamDeviceHandler deviceMonitor = container.Resolve<IScanSpamDeviceHandler>();

deviceMonitor.DataSaveStrategy = container.Resolve<IDataSaveStrategy>();

var device = container.Resolve<IScanSpamDevice>();
deviceMonitor.ConnectDevice(device);
deviceMonitor.StartReadFile(device, filePath);

//=======Code prototypes========================================================================