package com.example.zoldan.funnyjoke;

import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import java.io.BufferedReader;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.InetAddress;
import java.net.Socket;
import java.nio.charset.Charset;
import java.nio.charset.StandardCharsets;

public class MainActivity extends AppCompatActivity {

    String serIpAddress;       // адрес сервера
    int port = 10000;           // порт
    String msg;                 // Сообщение
    final byte codeMsg = 1;     // Оправить сообщение
    final byte codeRotate = 2;  // Повернуть экран
    final byte codePoff = 3;    // Выключить компьютер
    byte codeCommand;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    public void onClick (View v)
    {
        EditText etIPaddress = (EditText)findViewById(R.id.edIPaddress);
        serIpAddress = etIPaddress.getText().toString();
        if (serIpAddress.isEmpty()){
            Toast msgToast = Toast.makeText(this, "Введите ip адрес", Toast.LENGTH_SHORT);
            msgToast.show();
            return;
        }
        EditText etMsg = (EditText) findViewById(R.id.etMsg);
        msg = etMsg.getText().toString();
        SenderThread sender = new SenderThread();
     switch (v.getId())
     {
         case R.id.btnSMsg:
             if (!msg.isEmpty()) {
                 codeCommand = codeMsg;
                 sender.execute();
             }
             else {
                 Toast msgToast = Toast.makeText(this, "Введите сообщение", Toast.LENGTH_SHORT);
                 msgToast.show();
             }
             break;
         case R.id.btnRotate:
             codeCommand = codeRotate;
             sender.execute();
             break;
         case R.id.btnPowerOff:
             codeCommand = codePoff;
             sender.execute();
             break;
     }
    }

    class SenderThread extends AsyncTask <Void, Void, Void>
    {
        @Override
        protected Void doInBackground(Void... params) {
            try {
                InetAddress ipAddress = InetAddress.getByName(serIpAddress);
                Socket socket = new Socket(ipAddress, port);
                //InputStream inputStream = socket.getInputStream();
                OutputStream outputStream = socket.getOutputStream();
                DataOutputStream out = new DataOutputStream(outputStream);
                ///DataInputStream in = new DataInputStream(inputStream);
                switch (codeCommand) {
                    case codeMsg:
                        out.write(codeMsg);
                   //     out.flush();
                        byte[] outMsg = msg.getBytes("UTF8");
                        out.write(outMsg);
                        out.flush();
                        break;
                    case codeRotate:
                        out.write(codeRotate);
                     //   out.flush();
                        break;
                    case codePoff:
                        out.write(codePoff);
                       // out.flush();
                        break;
                }
            }
            catch (Exception ex)
            {
                ex.printStackTrace();
            }
            return null;
        }
    }
}
