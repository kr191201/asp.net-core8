// 로그인 버튼 클릭 시 실행하여 생성한 토큰값을 로컬 스토리지에 저장함
//async function fn_loginUser() {
//    var USEREMAIL = $("#USEREMAIL").val();
//    var USERPASSWORD = $("#USERPASSWORD").val();

//    try {
//        const response = await fetch('/Login/proc', {
//            method: 'POST',
//            headers: {
//                'Content-Type': 'application/json'
//            },
//            body: JSON.stringify({ USEREMAIL: USEREMAIL, USERPASSWORD: USERPASSWORD})
//        });

//        if (response.ok) { // 로그인 ok
//            const Token = await response.json();
//            localStorage.setItem('jwt', Token.token); // JWT 토큰 저장


//            document.getElementById("USEREMAIL").style.display = "none";
//            document.getElementById("USERPASSWORD").style.display = "none";

//            fn_GetUserInfo();

//        } else {
//            const errorData = await response.json();
//            console.error('로그인 실패:', errorData);
//        }
//    } catch (error) {
//        console.error('네트워크 오류:', error);
//    }
//}

//// 토큰 만료시간 체크
//function isTokenValid(token) {

//    if (!token) {
//        return false;
//    }

//    const payload = JSON.parse(atob(token.split('.')[1]));
//    const currentTime = Math.floor(Date.now() / 300000); // 현재 시간 (초)

//    return payload.exp > currentTime; // 만료 시간 체크
//}

//// 로그아웃 요청
//function fn_logout() {
//    fetch('/login/logout', {
//        method: 'GET'
//    })
//    .then(() => {
//        localStorage.removeItem('jwt'); // JWT 삭제
//    });
//}

//async function fn_GetUserInfo() {
//    const token = localStorage.getItem('jwt');
//    const response = await fetch('/login/GetUserInfo', {
//        method: 'GET',
//        headers: {
//            "Content-Type": "application/json",
//            'Authorization': `Bearer ${token}` // JWT 토큰을 Authorization 헤더에 포함
//        }
//    });

//    if (response.ok) {
//        const data = await response.json();
//        location.href = "/login/index2";
//        //return data;
//    } else {
//        console.error('Failed to fetch protected resource:', response.status);
//    }
//}


//document.addEventListener('DOMContentLoaded', () => {
//    fn_GetUserInfo();
//});


//######################################################################


async function fn_goChart() {
    //const token = localStorage.getItem('jwt'); // 저장된 JWT 가져오기

    const response = await fetch('/Board/Index2', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`, // JWT를 Authorization 헤더에 포함
            'Content-Type': 'application/json'
        }
    });

    if (response.ok) {
        const userInfo = await response.json();
        console.log(userInfo); // 사용자 정보 처리
    } else {
        console.error('Failed to fetch user info');
    }
}

// 로그인 버튼을 누르면 실행되는 이벤트
async function fn_loginProc() {
    var USEREMAIL = $("#USEREMAIL").val();
    var USERPASSWORD = $("#USERPASSWORD").val();

    const response = await fetch('https://localhost:7123/Login/proc', { // (id는 email, pw)로그인을 하면 토큰이 발급됨
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ USEREMAIL: USEREMAIL, USERPASSWORD: USERPASSWORD })
    });

    if (response.ok) {
        const data = await response.json();
        localStorage.setItem('jwt', data.token); // JWT 토큰 저장

        // 페이지 이동 전에 JWT 토큰을 포함한 요청을 보냄
        //const boardResponse = await fetch('/Home/Index2', { // 사용자 정보 가져옴
        //const boardResponse = await fetch('/Login/GetUserInfo', { // 사용자 정보 가져옴
        //    method: 'GET',
        //    headers: {
        //        'Authorization': 'Bearer ' + token
        //    }
        //});

        //if (boardResponse.ok) {
        //    //window.location.href = "/Board/Index2";
        //    //window.location.reload(); // 서버에서 사용자 정보를 성공적으로 가져왔으므로 페이지 새로고침
        //    //let jsonData = await boardResponse.json();
        //    //console.log(jsonData.userName);

        //} else {
        //    console.error('Failed to access Board/Index2');
        //}

        //window.location.href = "/Home/Index";
        //window.location.href = "/Board/Index2";
        fn_GetUserInfo(); // 로그인 성공 시 사용자 정보 가져오기
        //window.location.href = '/Home/Index';
    } else {
        console.error('Login failed');
    }
}

async function fn_GetUserInfo() {
    const token = localStorage.getItem('jwt');
    const response = await fetch('/Login/GetUserInfo', {
        method: 'GET',
        headers: {
            "Content-Type": "application/json",
            'Authorization': `Bearer ${token}` // JWT 토큰을 Authorization 헤더에 포함
        }
    });

    if (response.ok) {
        const data = await response.json();
        if (data && data.isAuthenticated) { // 인증까지 성공
            // 인증된 경우, /Home/Index로 페이지 이동
            window.location.href = '/Home/Index';
        } else {
            console.error('User is not authenticated');
        }
    } else {
        console.error('Failed to fetch protected resource:', response.status);
    }
}


//async function fn_GetUserInfo() {
//    const token = localStorage.getItem('jwt');
//    const response = await fetch('/Login/GetUserInfo', {
//        method: 'GET',
//        headers: {
//            "Content-Type": "application/json",
//            'Authorization': `Bearer ${token}` // JWT 토큰을 Authorization 헤더에 포함
//        }
//    });

//    if (response.ok) {
//        const data = await response.json();
//        if (data && data.isAuthenticated) {
//            // JWT 토큰을 포함하여 /Home/Index로 요청 보내기
//            const homeResponse = await fetch('/Home/Index', {
//                method: 'GET',
//                headers: {
//                    "Content-Type": "application/json",
//                    'Authorization': `Bearer ${token}` // JWT 토큰을 Authorization 헤더에 포함
//                }
//            });
//        } else {
//            console.error('User is not authenticated');
//        }
//    } else {
//        console.error('Failed to fetch protected resource:', response.status);
//    }
//}




//function fn_GetUserInfo() {

//    const token = localStorage.getItem('jwt');

//    await fetch('https://localhost:7123/Login/GetUserInfo', {
//        method: 'GET',
//        headers: {
//            'Authorization': `Bearer ${localStorage.getItem('jwt')}`
//        },
//        //credentials: 'include' // 쿠키를 포함하여 요청
//    })
//        .then(response => response.json())
//        .then(data => {
//            // 서버에서 받은 데이터 처리
//            console.log(data); // 받은 데이터 콘솔에 출력
//            //window.location.href = 'https://localhost:7123/Home/Index';
//            // 예시: 사용자 정보 표시
//            //const usernameElement = document.getElementById('username');
//            //usernameElement.textContent = data.username;

//            // 예시: 사용자 권한에 따라 UI 변경
//            //if (data.roles.includes('admin')) {
//            //    document.getElementById('admin-panel').style.display = 'block';
//            //}

//            // 예시: 데이터를 이용한 차트 그리기
//            //const chartData = data.chartData;
//            // 차트 라이브러리(e.g., Chart.js)를 이용하여 차트 생성

//        })
//        .catch(error => {
//            // 에러 처리
//            console.error('Error:', error);
//            // 사용자에게 에러 메시지 표시
//            alert('데이터를 가져오는 중 오류가 발생했습니다.');
//        });
//}