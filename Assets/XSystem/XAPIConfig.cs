using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONLib = SimpleJSON;

namespace XSystem {

    public interface IXAPIConfig {
        string Host();
        int Port();
        String URLEndPoint();
        string Version();
        int VersionCode();
    }

    public class XAPIConfig : IXAPIConfig {
        public enum EnvironmentEnum {
            Production,
            Development,
            Localhost
        }

        [SerializeField] public string host;
        [SerializeField] public int port;
        [SerializeField] public string version;
        [SerializeField] public int versionCode;
        
        public static XAPIConfig New(string host, int port, string version, int versionCode) {
            return new XAPIConfig() {
                host = host,
                port = port,
                version = version,
                versionCode = versionCode,
            };
        }
        
        public static XAPIConfig FromJSON(string json) {
            var jObj = JSONLib.JSON.Parse(json);
            var host = jObj["host"].Value;
            var port = jObj["port"].AsInt;
            var version = jObj["version"].Value;
            var versionCode = jObj["versionCode"].AsInt;
            return XAPIConfig.New(host, port, version, versionCode);
        }

        public string Host() {
            return host;
        }

        public int Port() {
            return port;
        }

        public string URLEndPoint() {
            if (this.host.StartsWith("https")) {
                return $"{this.host}";
            } else {
                return $"{this.host}:{this.port}";
            }
        }

        public string Version() {
            return version;
        }

        public int VersionCode() {
            return versionCode;
        }
    }

}