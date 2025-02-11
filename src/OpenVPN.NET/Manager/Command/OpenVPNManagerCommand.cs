﻿using System.Threading;
using System.Threading.Tasks;

namespace OpenVPN.NET.Manager {
    public class OpenVPNManagerCommand {
        public OpenVPNMessageStream LoggerStream { get; }

        public OpenVPNManagerCommand(OpenVPNMessageStream loggerStream) {
            LoggerStream = loggerStream;
        }
        
        /// <summary>
        /// 인증 실패 재시도 모드
        /// </summary>
        public async Task<OpenVPNReceiveInfo> SetAuthFailureRetryMode(ManagerAuthRetryMode mode, long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send($"auth-retry {mode.ToString().ToLower()}", timeout, token);

        /// <summary>
        /// 입출력 바이트 표시, sec 초마다 업데이트 (0 = off)
        /// </summary>
        public async Task<OpenVPNReceiveInfo> InOutByteCountUpdateEverySecond(uint sec, long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send($"bytecount {sec}", timeout, token);

        /// <summary>
        /// 로그와 비슷하지만 에코 버퍼의 메시지만 표시합니다
        /// </summary>
        public async Task<OpenVPNReceiveInfo> Echo(long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send("echo all", timeout, token);

        /// <summary>
        /// 로그와 비슷하지만 에코 버퍼의 메시지만 표시합니다
        /// </summary>
        public async Task<OpenVPNReceiveInfo> Echo(bool on, long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send($"echo {(on ? "on" : "off")}", timeout, token);

        /// <summary>
        /// 로그와 비슷하지만 에코 버퍼의 메시지만 표시합니다
        /// </summary>
        public async Task<OpenVPNReceiveInfo> Echo(uint n, long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send($"echo {n}", timeout, token);

        /// <summary>
        /// 지금까지 입력한 비밀번호를 잊어버립니다
        /// </summary>
        public async Task<OpenVPNReceiveInfo> ForgetPasswords(long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send($"forget-passwords", timeout, token);

        /// <summary>
        /// 명령 정보를 출력합니다
        /// </summary>
        public async Task<OpenVPNReceiveInfo> Help(long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send("help", timeout, token);

        /// <summary>
        /// 보류 플래그를 On/Off 상태로 설정 / 표시합니다
        /// </summary>
        public async Task<OpenVPNReceiveInfo> Hold(ManagerHoldMode mode, long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send($"hold {mode.ToString().ToLower()}", timeout, token);

        /// <summary>
        /// 전역 서버 부하 통계를 표시합니다
        /// </summary>
        public async Task<OpenVPNReceiveInfo> LoadStats(long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send("load-stats", timeout, token);

        /// <summary>
        /// 실시간 로그 표시 On/Off
        /// </summary>
        public async Task<OpenVPNReceiveInfo> Log(bool on, long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send($"log {(on ? "on" : "off")}", timeout, token);

        /// <summary>
        /// 전체 기록에 대해 마지막 N 줄을 표시합니다
        /// </summary>
        public async Task<OpenVPNReceiveInfo> Log(uint n, long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send($"log {n}", timeout, token);

        /// <summary>
        /// 전체 로그를 표시합니다
        /// </summary>
        public async Task<OpenVPNReceiveInfo> Log(long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send("log all", timeout, token);

        /// <summary>
        /// level를 표시합니다
        /// </summary>
        public async Task<OpenVPNReceiveInfo> Mute(long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send("mute", timeout, token);

        /// <summary>
        /// 로그 음소거 수준을 level으로 설정합니다
        /// </summary>
        public async Task<OpenVPNReceiveInfo> Mute(byte level, long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send($"mute{level}", timeout, token);

        /// <summary>
        /// (Windows 전용) 네트워크 정보 및 라우팅 테이블을 표시합니다
        /// </summary>
        public async Task<OpenVPNReceiveInfo> Net(long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send("net", timeout, token);

        public async Task<OpenVPNReceiveInfo> Username(string username, long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send($"username Auth {OpenVPNUtility.Escape(username)}", timeout, token);

        public async Task<OpenVPNReceiveInfo> Password(string password, long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send($"password Auth {OpenVPNUtility.Escape(password)}", timeout, token);

        /// <summary>
        /// 상태
        /// </summary>
        /// <param name="all"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<OpenVPNStateMessageInfo> GetState(long timeout = 20, CancellationToken? token = null) {
            var r = await LoggerStream.Send("state", timeout, token);
            r.ThrowErrorException();
            return new(r.Message);
        }

        /// <summary>
        /// pid, 실패시 음수 반환
        /// </summary>
        public async Task<int> GetPid(long timeout = 20, CancellationToken? token = null) {
            var r = await LoggerStream.Send("pid", timeout, token);
            r.ThrowErrorException();
            if (OpenVPNRequest.Parse(r.Message)
                .TryGetValue("pid", out var p) && int.TryParse(p, out var pid))
                return pid;
            return -1;
        }

        /// <summary>
        /// 시그널을 보냅니다
        /// </summary>
        public async Task<OpenVPNReceiveInfo> SendSignal(ManagerSignal signal, long timeout = 20, CancellationToken? token = null) =>
            await LoggerStream.Send($"signal SIG{signal.ToString().ToUpper()}", timeout, token);
    }
}
