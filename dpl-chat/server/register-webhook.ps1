Start-Process -FilePath .\ngrok\ngrok.exe -Args "http 3000" -passthru
start-sleep 1
$url = (Invoke-RestMethod -Uri http://127.0.0.1:4040/api/tunnels).tunnels[0].public_url;
$url = $url + "/stream-webhook";
#npx stream config:set -n Niko -e niko@binoinc.com -m development -k ryh6nu56cr6n -s xdb3zfvj4kxxcmdf9pkbv9sdp4hk4b3zczmr2a8vnuu4e82yjxhdrxud5735wkn2 -u https://chat-us-east-1.stream-io-api.com -t true
npx stream chat:push:webhook --url $url
echo $url