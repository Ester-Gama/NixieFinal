using UnityEngine;
using Android.BLE;
using Android.BLE.Commands;
using UnityEngine.Android;
using UnityEngine.UI;
using System.Text;
using TMPro;

public class ExampleBleInteractor : MonoBehaviour
{
    //aqui
    [SerializeField]
    TextMeshProUGUI status;
    [SerializeField]
    string nomeDispositivo;
    [SerializeField]
    private string _servico = "ffe0", _caracteristica = "ffe1";

    [SerializeField]
    private int _scanTime = 10;

    private float _scanTimer = 0f;

    private bool _isScanning = false;

    private ConnectToDevice _connectCommand;

    private string _deviceUuid = string.Empty;

    public SubscribeToCharacteristic sb;

    public void ScanForDevices()
    {
        if (!_isScanning)
        {
            _isScanning = true;
            BleManager.Instance.QueueCommand(new DiscoverDevices(OnDeviceFound, _scanTime * 1000));
        }
    }

    private void Update()
    {
        if(_isScanning)
        {
            _scanTimer += Time.deltaTime;
            if(_scanTimer > _scanTime)
            {
                _scanTimer = 0f;
                _isScanning = false;
            }
        }
    }

    private void OnDeviceFound(string mac, string nome)
    {


        // DeviceButton button = Instantiate(_deviceButton, _deviceList).GetComponent<DeviceButton>();
        //status.text += "Nome: " + name + " Device: " + device+"\n";



        if (nome== nomeDispositivo)
        {
            status.text = "Nome: " + nome + " Device: " + mac;
            _deviceUuid = mac;
            _connectCommand = new ConnectToDevice(_deviceUuid, OnConnected, OnDisconnected);
            BleManager.Instance.QueueCommand(_connectCommand);
 
}
    }


    private void OnConnected(string deviceUuid)
    {
        status.text = "subescrevendo";
        SubscribeToExampleService();
    }

    private void OnDisconnected(string deviceUuid)
    {
      //  _deviceButtonImage.color = _previousColor;
      //
      //  _isConnected = false;
      //  _deviceButtonText.text = "Connect";
    }
    public void SubscribeToExampleService()
    {
        //aqui
        
        SubscribeServico(_deviceUuid);
    }

    public void SubscribeServico(string _dvcUuid)
    {
        //aqui
        //ATENÇÃO, BLUETOOTH LOW ENERGY SÓ RECEBE 20 BYTES DE CADA VEZ, CONTANDO \r\n
        _deviceUuid = _dvcUuid;
        sb = new SubscribeToCharacteristic(_deviceUuid, _servico, _caracteristica, (byte[] value) =>
        {
            status.text = Encoding.ASCII.GetString(value);
        });
        BleManager.Instance.QueueCommand(sb);
        sb.Start();
    }
    public void EnviarOn()
    {
        //ATENÇÃO, BLUETOOTH LOW ENERGY SÓ TRANSMITE 20 BYTES DE CADA VEZ, CONTANDO \r\n
        WriteToCharacteristic w = new WriteToCharacteristic(_deviceUuid, _servico, _caracteristica, "on\n");
        w.Start();
    }
    public void EnviarOff()
    {
        //ATENÇÃO, BLUETOOTH LOW ENERGY SÓ TRANSMITE 20 BYTES DE CADA VEZ, CONTANDO \r\n
        WriteToCharacteristic w = new WriteToCharacteristic(_deviceUuid, _servico, _caracteristica, "off\n");
        w.Start();
    }



}
