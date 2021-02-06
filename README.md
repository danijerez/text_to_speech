# Text to speech - Web Scraping

<img src="img/img.png" width="100%"/> 

En este blog, voy a explicar como hacer un `text to speech`, texto a voz, en unas pocas lineas de código sin ningún tipo de registro/pago del estilo Google, Amazon, etc. El único límite son 6000 caracteres a la semana, al menos con la alternativa que yo he probado. Cada vez se están poniendo mas de moda este tipo de paginas que de forma gratuita nos permiten usar de forma `online` herramientas que nos facilitan la vida. Es el caso, por ejemplo, de `freetts.com` que permite transformar un texto a audio en distintos idiomas y voces con el límite de 6000 caracteres semanales en su version gratuita. Este servicio por debajo utiliza por debajo la tecnología de  `Google machine learning and TTS capability` dando resultados bastante buenos.

Para el ejemplo he utilizado el lenguaje `C#` y la tecnología `.net core`, pero se puede reescribir en cualquier lenguaje, yo por ejemplo lo uso en un pequeño bot de `Telegram`.

Para empezar, iremos a `freetts.com` y con una configuración sencilla de mensaje, lenguaje y estilo de voz al darle a convertir, podemos inspeccionar la petición que se hace, en mi caso hizo esta:

~~~
https://freetts.com/Home/PlayAudio?Language=es-US&Voice=Penelope_Female&TextMessage=hola%20mundo&id=Penelope&type=1
~~~

Si volvemos a inspeccionar la página de respuesta, vemos un bloque `div` con el `_audio` con un path al .mp3 generado a partir de nuestro texto. ej: `"/audio/c1dd2730-eafc-42bf-b375-d0eff50e5d28.mp3"`

~~~
<div id="audio_">
    <div class="text-center" style="margin-top:10px">
        <audio controls="controls">
            <source src="/audio/c1dd2730-eafc-42bf-b375-d0eff50e5d28.mp3" type="audio/mpeg" />
        </audio>
    </div>
    <div class="text-center">
        <a href="/Home/download?path=c1dd2730-eafc-42bf-b375-d0eff50e5d28.mp3">Click to download audio</a>
    </div>
</div>
~~~

Os dejo una función que escribí que hace todo esto conjuntamente y obtiene unicamente la url donde esta el sonido. Para el web scraping del bloque `div` simplemente voy moviéndome por los nodos hasta llegar al atributo `src` que contiene el path.

~~~
public static string TextToSpeech(string message)
{
    
    var client = new RestClient(url);
    var request = new RestRequest("Home/PlayAudio", Method.GET);

    request.AddQueryParameter("Language", language);
    request.AddQueryParameter("Voice", voice);
    request.AddQueryParameter("TextMessage", message);
    request.AddQueryParameter("id", id);
    request.AddQueryParameter("type", type);

    var response = client.Execute(request);

    var doc = new HtmlDocument();
    doc.LoadHtml(response.Content);
    var pathMp3 = doc.GetElementbyId("audio_").ChildNodes[1].ChildNodes[1].ChildNodes[1].Attributes["src"].Value;

    return url + pathMp3;
}
~~~

Este fichero se borra en 24h de los sistemas, pero podemos descargarlo por ejemplo con una función como esta.

~~~
public static void DownloadMp3(string url)
{
    WebClient client = new WebClient();
    byte[] data = client.DownloadData(new Uri(url));
    File.WriteAllBytes(pathOut, data);
}
~~~

Quizá no es la mejor alternativa para usar este tipo de herramientas por sus limites, pero para hacer proyectos pequeños y de aprendizaje creo que merece la pena compartirlo. Os dejo alternativas online que hacen lo mismo. ¡Espero que os sea útil!


|Website|Link|Overview|
|--- |--- |--- |
|FreeTTS|https://freetts.com|30 Languages/Voices. Save as Mp3 file. No Sign up. 6000 character per day for free.|
|TTS Reader|https://ttsreader.com|20 Languages/Voices. Sign up for downloading and premium features. Pay for commercial use.|
|NaturalReaders|https://www.naturalreaders.com/online/|13 Languages/Voices. Save as Mp3 file. 20 min/day for Premium Voices. Sign up and pay for premium plan|
|iSpeech|https://www.ispeech.org/text.to.speech|27 Languages/Voices. Sign up for downloading. Provide APIs for developers.|
|From Text to Speech|http://www.fromtexttospeech.com/|8 Languages/Voices. Save as Mp3. 50000 characters for each user.|
|Text 2 Speech|https://www.text2speech.org/|4 Languages/Voices. Save as Mp3/WAV.|
|Notevibes|https://notevibes.com/|23 Languages/Voices. Save as Mp3. 200 characters and personal use for free users.|
|TTS Mp3|https://ttsmp3.com/|24 Languages/Voices. Save as Mp3. 5000 characters per day for free use.|
|ReadSpeaker|https://www.readspeaker.com/|35 Languages/Voices. Can not download as Mp3.|
|Google Text-to-Speech|https://play.google.com/store/apps/details?id=com.google.android.tts&hl=en_US|Android App.|
|Neospeech.com|https://neospeech.com/|14 Languages/Voices. Can not download as Mp3. Provide SDK for developers.|
|ReadtheWords|https://www.readthewords.com/|4 Languages/Voices. Save as Mp3. 100 characters for free use.|
|TTSDemo|http://ttsdemo.com/|29 Languages/Voices. Save as Mp3. 600 characters for free use.|
|Text2Mp3|https://www.texttomp3.online/|7 Languages/Voices. Save as Mp3. 300 characters for free use.|
|TexttoSpeech|https://texttospeech.io/|30 Languages/Voices. Sign up for downloading. 100 characters for free use. Bad ads layout.|
|Inforobo|https://inforobo.com/text-to-speech-online/|30 Languages/Voices. Can not download as Mp3.|
|Vozme|http://vozme.com|2 Languages/Voices. Save as Mp3.|
