// thetextfreshserver.cpp : 定義主控台應用程式的進入點。
//

#include "stdafx.h"
#define WIN32_LEAN_AND_MEAN


#include <stdio.h>
#include <stdlib.h>
#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <process.h>
#include <string.h>
#include <direct.h>
#include <io.h>
#include <locale.h> 


#pragma comment (lib, "Ws2_32.lib")
#pragma comment (lib, "Mswsock.lib")
#pragma comment (lib, "AdvApi32.lib")
HANDLE action_after_accepted_sock_work[300];
struct args_set {
	SOCKET accepted_sock;
	struct timeval out_thetime;
	int work_number;
};
void ascii_convert(char* beconvert, wchar_t* convert) {

	int len = MultiByteToWideChar(CP_ACP, 0, beconvert, -1, convert, MAX_PATH);
}
void unicode_convert(wchar_t* beconvert, char* convert) {

	int len = WideCharToMultiByte(950, 0, beconvert, -1, convert, wcslen(beconvert) * 2 , NULL, NULL);
}
void utf8_convert(char* beconvert, char* convert) {
	wchar_t *sub = (wchar_t*)malloc(strlen(beconvert) * 2);
	int length = MultiByteToWideChar(CP_UTF8, 0, beconvert, -1, sub, MAX_PATH);
	int len = WideCharToMultiByte(950, 0, sub, -1, convert, wcslen(sub) * 2 , NULL, NULL);
}
void utf8_unicode_convert(char* beconvert, wchar_t* convert) {

	int length = MultiByteToWideChar(CP_UTF8, 0, beconvert, -1, convert, MAX_PATH);	
}
unsigned __stdcall handle_other_thing(void* the_p) {
	args_set* the_args = (args_set*)the_p;
	SOCKET connect_client = the_args->accepted_sock;
	char buff[1024] = { '\0' };
	int the_sock_num = the_args->work_number;
	struct timeval timesout = the_args->out_thetime;
	fd_set rec_fd;
	FILE *nowfile;
	FD_ZERO(&rec_fd);
	FD_SET(connect_client, &rec_fd);
	fd_set loop_fd;
	struct _finddata_t find_the_file;
	bool filename_reach = false;
	bool filecommand = false;
	bool save_file_bool = false;
	int file_finding = 0;
	char file_name[] = "*.txt";
	char show[1024] = { '\0' };
	int sub_num;
	char show_file_name[1024] = { '\0' };
	char the_file[50000] = { '\0' };
	char the_big_buff[50000] = { '\0' };
	while (true) {
		loop_fd = rec_fd;
		if (select(1, &loop_fd, NULL, NULL, &timesout) > 0) {

		}
		for (int i = 0; i < 1500; ++i) {
		  	if (FD_ISSET(i, &loop_fd)) {
				int rec = recv(connect_client, buff, sizeof(buff), 0);
				char comp[] = "read the filelist";
				char comp2[] = "send the file";
				wchar_t trans_encode[1024] = { '\0' };
				printf(buff);
				if (strncmp(buff ,comp, 17) == 0){
						printf("filecommand is true");
						filecommand = true;
					}
					if (strncmp(buff, comp2, 13) == 0) {
						printf("save file bool is true");
						save_file_bool = true;
					}												
					if (!filecommand&&!save_file_bool) {
						utf8_convert(buff, show_file_name);
						ZeroMemory(&buff, sizeof(buff));
						printf(show_file_name);
						fopen_s(&nowfile,show_file_name, "a+");
						while (fgets(buff, 400, nowfile)) {

							if (buff[strlen(buff) - 1] == '\n') {
								printf("new line");
								buff[strlen(buff) - 1] == '\n';
							}
							
							ascii_convert(buff, trans_encode);
							send(connect_client, (char*)trans_encode, wcslen(trans_encode) * 2, 0);
							Sleep(5);
							printf(buff);
						}
						fclose(nowfile);
						sub_num = send(connect_client, (char*)L"end", 6, 0);
						filename_reach = true;	
						ZeroMemory(&trans_encode, sizeof(trans_encode));
						ZeroMemory(&buff, sizeof(buff));
						ZeroMemory(&nowfile, sizeof(nowfile));
					}				
				if (save_file_bool) {
					ZeroMemory(&buff, sizeof(buff));					
					ZeroMemory(&the_file, sizeof(the_file));					
					fopen_s(&nowfile, show_file_name, "w+");
					while (true) {
						int re = recv(connect_client, the_file,sizeof(the_file), 0);
						if (strncmp(the_file, "end", 3) != 0) {							
							utf8_convert(the_file, the_big_buff);
							printf(the_big_buff);
							fputs(the_big_buff,nowfile);
							Sleep(5);
						}
						else
						{
							printf("got close file");
							int iii = fclose(nowfile);
							printf(" %d ", iii);
							save_file_bool = false;
							ZeroMemory(&nowfile, sizeof(nowfile));
							break;
						}
					}
				}
				if (filecommand) {
					
					file_finding = _findfirst(file_name, &find_the_file);
					printf("%d ", file_finding);
					if (file_finding != -1) {
						
						ascii_convert(find_the_file.name, trans_encode);
						unicode_convert(trans_encode, show);
						printf(show);
						//printf(find_the_file.name);	
						sub_num = send(connect_client, (char*)trans_encode, (wcslen(trans_encode))*2, 0);
						while (_findnext(file_finding, &find_the_file) == 0) {	
							Sleep(20);
							ZeroMemory(&show, sizeof(show));
							ascii_convert(find_the_file.name, trans_encode);
							//printf(find_the_file.name);
							unicode_convert(trans_encode, show);
							printf(show);
							sub_num = send(connect_client, (char*)trans_encode, (wcslen(trans_encode))*2, 0);
						}
						
					}
					sub_num = send(connect_client, (char*)L"end", 6, 0);						
					filename_reach = true;	
					filecommand = false;
									
				}
				ZeroMemory(&buff, sizeof(buff));	
			}
		}
	}

	return 0;
}
int main()
{
	WSADATA  wsaData;
	int iResult;
	iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
	if (iResult != 0) {
		printf("WSAStartup failed with error: %d\n", iResult);
		return 1;
	}
	args_set for_acceptsock;
	fd_set read_some;
	fd_set the_fd;
	int maxline=30;
	char the_buff[1024] = { '\0' };
	int work_number = 0;
	struct addrinfo *ptr = NULL;
	struct addrinfo hints;
	ZeroMemory(&hints, sizeof(hints));
	ZeroMemory(&ptr, sizeof(ptr));
	hints.ai_family = AF_INET;
	hints.ai_protocol = IPPROTO_TCP;
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_flags = AI_PASSIVE;
	ptr = &hints;
	int addrinfo_get = getaddrinfo(NULL, "8080", &hints, &ptr);
	printf("%d", addrinfo_get);
	int un_connect_socket = socket(ptr->ai_family, ptr->ai_socktype, ptr->ai_protocol);
	int socket_bind = bind(un_connect_socket, ptr->ai_addr, ptr->ai_addrlen);
	int listen_client = listen(un_connect_socket, 300);
	int will_be_accept;
	printf("%d %d %d", un_connect_socket, socket_bind, listen_client);
	fd_set the_accept;
	fd_set read_string;
	struct timeval out_time;
	FD_ZERO(&the_accept);
	FD_ZERO(&read_string);
	FD_ZERO(&read_some);
	FD_ZERO(&the_fd);
	FD_SET(un_connect_socket, &the_fd);
	out_time.tv_sec = 0;
	out_time.tv_usec = 10;
	fd_set the_accept_nobuff;
	FD_ZERO(&the_accept_nobuff);
     while(true){
		read_some = the_fd;
		the_accept = the_accept_nobuff;
		if (select(maxline + 1, &read_some, NULL, NULL, &out_time) > 0) {
			printf("read some");
		}		
		if (select(maxline + 1, &the_accept, NULL, NULL, &out_time) > 0 ) {
			printf("the accept");
		}

		for (int i = 0; i < 1500; ++i) {												
			if (FD_ISSET(i,&read_some)){
				will_be_accept = accept(un_connect_socket, NULL, NULL);
				for_acceptsock.accepted_sock = will_be_accept;
				for_acceptsock.out_thetime = out_time;
				for_acceptsock.work_number = work_number;
				action_after_accepted_sock_work[work_number] = (HANDLE)_beginthreadex(NULL, 0, handle_other_thing, &for_acceptsock, 0, 0);
				work_number++;
			}	
			
	}
}
    return 0;
}

