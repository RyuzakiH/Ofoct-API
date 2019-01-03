# Ofoct-API
Unofficial API for [Ofoct/AudioToText](https://www.ofoct.com/audio-converter/audio-to-text.html) in .NET

# Website Tool Description
This is an online tool for recognition audio voice file (mp3, wav, ogg, wma) to text.<br /><br />
**Supported audio file formats: mp3, wav, wma, ogg**

# Usage

```csharp
var audioToText = new AudioToText();
```

First Way
```csharp
// upload a file from your pc
var application = audioToText.UploadFile("application.ogg");

// convert the aduio file to text
var application_baidu = audioToText.Convert(application, AudioToText.Engine.Baidu);
var application_cmu = audioToText.Convert(application, AudioToText.Engine.CMU_Sphinx);
```
Second Way
```csharp
// convert the audio file directly
var application = audioToText.Convert("application.ogg", AudioToText.Engine.Baidu);
```
