MD "Release\"

C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe -target:winexe -optimize -unsafe -out:"Release\Fast Patch-based Style Transfer of Arbitrary Style.exe" -r:"System.dll" -r:"System.Drawing.dll" -r:"System.Threading.dll" -r:"System.Threading.Tasks.dll" -r:"System.IO.dll" -r:"System.Windows.Forms.dll" -r:"System.Reflection.dll"  -resource:"src\Resources\Content.jpg" -resource:"src\Resources\dec-tconv-sigmoid.model" -resource:"src\Resources\MainIcon.jpg" -resource:"src\Resources\Style.jpg" -resource:"src\Resources\vgg19.model" "src\Data\DecoderData.cs" "src\Data\EncoderData.cs" "src\Interface\MainForm.cs" "src\Layers\Conv2DLayer.cs" "src\Layers\ConvTranspose2DLayer.cs" "src\Layers\InstanceNorm2DLayer.cs" "src\Layers\MaxPool2DLayer.cs" "src\Layers\ReLULayer.cs" "src\Layers\SigmoidLayer.cs" "src\Layers\StyleSwap.cs" "src\Layers\Upsample2DLayer.cs" "src\Network\Decoder.cs" "src\Network\Encoder.cs" "src\Network\Stylize.cs" "src\Utils\IOConverters.cs" "src\Utils\Tensor.cs" "src\Program.cs" "src\Properties.cs"

cmd.exe