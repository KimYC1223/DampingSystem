const express = require('express');
const path = require('path');
const app = express();
const port = 3000;

// WebGL 빌드 파일 디렉토리를 명확히 지정 (현재 디렉토리 기준)
app.use(express.static(path.join(__dirname, '/')));

// 기본 루트 요청에만 index.html을 반환하도록 명확히 지정
app.get('/', (req, res) => {
    res.sendFile(path.join(__dirname, 'index.html'));
});

// 서버 시작
app.listen(port, () => {
    console.log(`WebGL 서버 실행 중: http://localhost:${port}`);
});
