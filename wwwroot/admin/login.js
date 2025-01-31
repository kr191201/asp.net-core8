async function fn_goChart() {
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

    const response = await fetch('/Login/Proc', { // (id는 email, pw)로그인을 하면 토큰이 발급됨
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ USEREMAIL: USEREMAIL, USERPASSWORD: USERPASSWORD })
    });

    if (response.ok) {
        const data = await response.json();
        localStorage.setItem('jwt', data.token); // JWT 토큰 저장

        fn_GetUserInfo(); // 로그인 성공 시 사용자 정보 가져오기
    } else {
        alert("로그인 실패");
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
        // 서버에서 리다이렉트 URL을 반환한다고 가정
        const redirectUrl = await response.json(); // 서버에서 리다이렉트 URL을 JSON으로 반환
        window.location.href = redirectUrl.redirectUrl; // 해당 URL로 이동
    } else {
        console.error('Failed to fetch user info:', response.statusText);
    }
}