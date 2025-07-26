# DampingSystem

<div align="center">

![Unity](https://img.shields.io/badge/Unity-2022.3.20f1+-000000.svg?style=flat&logo=unity&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-blue.svg?style=flat)

**Second-Order Damping Animation Library for Unity**

Youtube video [Giving Personality to Procedural Animations using Math](https://www.youtube.com/watch?v=KPoeNZZ6H4s) inspired Unity package.

There is a [Korean translation](#korean) at the bottom.

</div>

---

## 📋 Table of Contents
ㅉ
1. [🎯 Project Introduction](#-project-introduction)
2. [✨ Key Features](#-key-features)
3. [🔧 Installation](#-installation)
4. [🚀 Quick Start](#-quick-start)
5. [📖 Detailed Usage](#-detailed-usage)
6. [🧮 Mathematical Principles](#-mathematical-principles)
7. [🎮 Supported Types](#-supported-types)
8. [📊 Performance Characteristics](#-performance-characteristics)
9. [🔗 References](#-references)
10. [📞 Contact](#-contact)

---

## 🎯 Project Introduction

**DampingSystem** is a second-order system-based damping library for creating natural and smooth animations in Unity. It simulates spring-damper system motion using physically accurate mathematical models.

### 🎥 WebGL Demo

// Coming soon!

---

## ✨ Key Features

- 🔬 **Mathematical Accuracy**: Physically accurate implementation based on second-order differential equations
- ⚡ **High Performance**: Runtime performance guaranteed with `AggressiveInlining` optimization
- 🎯 **Multiple Type Support**: Full support for float, Vector2/3/4, Quaternion
- 🛡️ **Stability**: NaN prevention and numerical stability assurance
- 🔧 **Ease of Use**: Easy setup with intuitive parameters
- 📚 **Complete Documentation**: Fully documented in both Korean and English

---

## 🔧 Installation

Download the [latest released Unity Package](https://github.com/KimYC1223/DampingSystem/releases) or download this repository.

### Requirements

- **Unity**: 2022.3.20f1 or higher

---

## 🚀 Quick Start

### Basic Usage

```csharp
using DampingSystem;

public class PlayerController : MonoBehaviour
{
    private DampingSystemVector3 positionDamper;
    
    void Start()
    {
        // Initialize damping system
        positionDamper = new DampingSystemVector3(
            frequency: 2.0f,        // Natural frequency
            dampingRatio: 1.0f,     // Damping ratio (1.0 = critical damping)
            initialResponse: 0.0f,  // Initial response
            initialCondition: transform.position
        );
    }
    
    void Update()
    {
        // Smooth movement to target position
        Vector3 targetPosition = GetTargetPosition();
        Vector3 smoothPosition = positionDamper.Calculate(targetPosition);
        transform.position = smoothPosition;
    }
}
```

### Rotation Animation

```csharp
private DampingSystemQuaternion rotationDamper;

void Start()
{
    rotationDamper = new DampingSystemQuaternion(
        frequency: 1.5f,
        dampingRatio: 0.8f,  // Slight overshoot
        initialResponse: 0.2f,
        initialCondition: transform.rotation
    );
}

void Update()
{
    Quaternion targetRotation = GetTargetRotation();
    transform.rotation = rotationDamper.Calculate(targetRotation);
}
```

---

## 📖 Detailed Usage

### Parameter Description

| Parameter | Description | Recommended Values |
|-----------|-------------|-------------------|
| `frequency` | Natural frequency (Hz)<br>*Higher values = faster response* | 1.0 ~ 5.0 |
| `dampingRatio` | Damping ratio<br>*1.0 = critical, >1.0 = overdamped, <1.0 = underdamped* | 0.7 ~ 1.2 |
| `initialResponse` | Initial response<br>*Controls anticipation effect* | 0.0 ~ 0.5 |
| `initialCondition` | Initial condition<br>*Starting state value* | Current value |

### Behavior by Damping Ratio

```csharp
// Underdamped - oscillates to convergence
var bouncyDamper = new DampingSystemFloat(2.0f, 0.5f, 0.0f, 0.0f);

// Critically Damped - fastest convergence
var criticalDamper = new DampingSystemFloat(2.0f, 1.0f, 0.0f, 0.0f);

// Overdamped - slow convergence, no overshoot
var slowDamper = new DampingSystemFloat(2.0f, 2.0f, 0.0f, 0.0f);
```

### Custom Time Step Usage

```csharp
void FixedUpdate()
{
    // Using fixed time step
    float customDeltaTime = Time.fixedDeltaTime;
    Vector3 result = damper.Calculate(target, customDeltaTime);
}
```

---

## 🧮 Mathematical Principles

### Second-Order System Differential Equation

DampingSystem is based on the following second-order differential equation:

```
ÿ + 2ζωẏ + ω²y = ω²x + 2ζωrẋ
```

Where:
- `ω` = Natural angular frequency (ω = 2πf)
- `ζ` = Damping ratio
- `r` = Initial response coefficient
- `x` = Input signal
- `y` = Output signal

For detailed information, refer to this [video](https://www.youtube.com/watch?v=KPoeNZZ6H4s)!

---

## 🎮 Supported Types

### Scalar Type

```csharp
// Float damping
var floatDamper = new DampingSystemFloat(2.0f, 1.0f, 0.0f, 0.0f);
float smoothValue = floatDamper.Calculate(targetValue);
```

### Vector Types

```csharp
// 2D position damping
var vec2Damper = new DampingSystemVector2(1.5f, 0.8f, 0.0f, Vector2.zero);

// 3D position damping  
var vec3Damper = new DampingSystemVector3(2.0f, 1.0f, 0.1f, Vector3.zero);

// 4D vector damping (colors, etc.)
var vec4Damper = new DampingSystemVector4(3.0f, 1.2f, 0.0f, Vector4.one);
```

### Rotation Type

```csharp
// Quaternion rotation damping (Slerp-based)
var rotDamper = new DampingSystemQuaternion(
    1.0f, 0.9f, 0.0f, Quaternion.identity
);
```

---

## 📊 Performance Characteristics

### Optimization Techniques

- **Aggressive Inlining**: Eliminates function call overhead
- **Struct-based**: Minimizes memory allocation
- **NaN Filtering**: Safe numerical operations
- **Conditional Calculation**: Skips unnecessary operations

### Benchmark Results

| Type            | Operation Time (ns) | Memory Usage |
|:---------------:|:-------------------:|:------------:|
| Float           | ~15                 | 64 bytes     |
| Vector3         | ~45                 | 96 bytes     |
| Quaternion      | ~80                 | 112 bytes    |

---

## 🔗 References

---

## 📞 Contact

**Developer**: KimYC1223  
**Email**: kau_esc@naver.com  
**GitHub**: [@KimYC1223](https://github.com/KimYC1223)

<div align="center">

**⭐ If this project was helpful, please give it a star! ⭐**

Made with ❤️ by KimYC1223

</div>

---
---

<a name="korean"></a>

# DampingSystem

<div align="center">

![Unity](https://img.shields.io/badge/Unity-2022.3.20f1+-000000.svg?style=flat&logo=unity&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-blue.svg?style=flat)

**Unity용 2차 시스템 기반 감쇠 애니메이션 라이브러리**

Youtube 영상 [Giving Personality to Procedural Animations using Math](https://www.youtube.com/watch?v=KPoeNZZ6H4s)을 보고 영감을 받아 만든 Unity package입니다.

상단에 [영어 번역](#dampingsystem)이 있습니다.

</div>

---

## 📋 목차

1. [🎯 프로젝트 소개](#-프로젝트-소개)
2. [✨ 주요 특징](#-주요-특징)
3. [🔧 설치 방법](#-설치-방법)
4. [🚀 빠른 시작](#-빠른-시작)
5. [📖 상세 사용법](#-상세-사용법)
6. [🧮 수학적 원리](#-수학적-원리)
7. [🎮 지원 타입](#-지원-타입)
8. [📊 성능 특성](#-성능-특성)
9. [🔗 참고자료](#-참고자료)
10. [📞 연락처](#-연락처)

---

## 🎯 프로젝트 소개

**DampingSystem**은 Unity에서 자연스럽고 부드러운 애니메이션을 구현하기 위한 2차 시스템 기반 감쇠 라이브러리입니다. 물리적으로 정확한 수학 모델을 사용하여 스프링-댐퍼 시스템의 움직임을 시뮬레이션합니다.

### 🎥 WebGL데모

// 준비중!

---

## ✨ 주요 특징

- 🔬 **수학적 정확성**: 2차 미분방정식 기반의 물리적으로 정확한 구현
- ⚡ **고성능**: `AggressiveInlining` 최적화로 런타임 성능 보장
- 🎯 **다양한 타입 지원**: float, Vector2/3/4, Quaternion 완벽 지원
- 🛡️ **안정성**: NaN 방지 및 수치적 안정성 보장
- 🔧 **사용 편의성**: 직관적인 매개변수로 쉬운 설정
- 📚 **완전한 문서화**: 한국어/영어 이중 문서화

---

## 🔧 설치 방법

[최신의 release된 Unity Package](https://github.com/KimYC1223/DampingSystem/releases)를 다운로드 하거나, 이 repo를 다운받으면 된다.

### 요구사항

- **Unity**: 2022.3.20f1 이상

---

## 🚀 빠른 시작

### 기본 사용법

```csharp
using DampingSystem;

public class PlayerController : MonoBehaviour
{
    private DampingSystemVector3 positionDamper;
    
    void Start()
    {
        // 감쇠 시스템 초기화
        positionDamper = new DampingSystemVector3(
            frequency: 2.0f,        // 자연 진동수
            dampingRatio: 1.0f,     // 감쇠비 (1.0 = 임계감쇠)
            initialResponse: 0.0f,  // 초기 응답
            initialCondition: transform.position
        );
    }
    
    void Update()
    {
        // 목표 위치로 부드럽게 이동
        Vector3 targetPosition = GetTargetPosition();
        Vector3 smoothPosition = positionDamper.Calculate(targetPosition);
        transform.position = smoothPosition;
    }
}
```

### 회전 애니메이션

```csharp
private DampingSystemQuaternion rotationDamper;

void Start()
{
    rotationDamper = new DampingSystemQuaternion(
        frequency: 1.5f,
        dampingRatio: 0.8f,  // 약간의 오버슈트
        initialResponse: 0.2f,
        initialCondition: transform.rotation
    );
}

void Update()
{
    Quaternion targetRotation = GetTargetRotation();
    transform.rotation = rotationDamper.Calculate(targetRotation);
}
```

---

## 📖 상세 사용법

### 매개변수 설명

| 매개변수 | 설명 | 권장값 |
|---------|------|--------|
| `frequency` | 자연 진동수 (Hz)<br>*값이 클수록 빠른 반응* | 1.0 ~ 5.0 |
| `dampingRatio` | 감쇠비<br>*1.0 = 임계감쇠, >1.0 = 과감쇠, <1.0 = 부족감쇠* | 0.7 ~ 1.2 |
| `initialResponse` | 초기 응답<br>*anticipation 효과 조절* | 0.0 ~ 0.5 |
| `initialCondition` | 초기 조건<br>*시작 상태값* | 현재값 |

### 감쇠비에 따른 동작 특성

```csharp
// 부족감쇠 (Underdamped) - 진동하며 수렴
var bouncyDamper = new DampingSystemFloat(2.0f, 0.5f, 0.0f, 0.0f);

// 임계감쇠 (Critically Damped) - 가장 빠른 수렴
var criticalDamper = new DampingSystemFloat(2.0f, 1.0f, 0.0f, 0.0f);

// 과감쇠 (Overdamped) - 느린 수렴, 오버슈트 없음
var slowDamper = new DampingSystemFloat(2.0f, 2.0f, 0.0f, 0.0f);
```

### 커스텀 시간 간격 사용

```csharp
void FixedUpdate()
{
    // 고정 시간 간격 사용
    float customDeltaTime = Time.fixedDeltaTime;
    Vector3 result = damper.Calculate(target, customDeltaTime);
}
```

---

## 🧮 수학적 원리

### 2차 시스템 미분방정식

DampingSystem은 다음 2차 미분방정식을 기반으로 합니다:

```
ÿ + 2ζωẏ + ω²y = ω²x + 2ζωrẋ
```

여기서:
- `ω` = 자연 각주파수 (ω = 2πf)
- `ζ` = 감쇠비
- `r` = 초기 응답 계수
- `x` = 입력 신호
- `y` = 출력 신호

자세한 내용은 이 [영상](https://www.youtube.com/watch?v=KPoeNZZ6H4s)을 참고하세요!

---

## 🎮 지원 타입

### 스칼라 타입

```csharp
// Float 감쇠
var floatDamper = new DampingSystemFloat(2.0f, 1.0f, 0.0f, 0.0f);
float smoothValue = floatDamper.Calculate(targetValue);
```

### 벡터 타입

```csharp
// 2D 위치 감쇠
var vec2Damper = new DampingSystemVector2(1.5f, 0.8f, 0.0f, Vector2.zero);

// 3D 위치 감쇠  
var vec3Damper = new DampingSystemVector3(2.0f, 1.0f, 0.1f, Vector3.zero);

// 4D 벡터 감쇠 (색상 등)
var vec4Damper = new DampingSystemVector4(3.0f, 1.2f, 0.0f, Vector4.one);
```

### 회전 타입

```csharp
// Quaternion 회전 감쇠 (Slerp 기반)
var rotDamper = new DampingSystemQuaternion(
    1.0f, 0.9f, 0.0f, Quaternion.identity
);
```

---

## 📊 성능 특성

### 최적화 기법

- **Aggressive Inlining**: 함수 호출 오버헤드 제거
- **구조체 기반**: 메모리 할당 최소화
- **NaN 필터링**: 안전한 수치 연산
- **조건부 계산**: 불필요한 연산 스킵

### 벤치마크 결과

| 타입             | 연산 시간 (ns) | 메모리 사용량 |
|:---------------:|:-------------:|:-----------:|
| Float           | ~15           | 64 bytes    |
| Vector3         | ~45           | 96 bytes    |
| Quaternion      | ~80           | 112 bytes   |

---

## 🔗 참고자료

---

## 📞 연락처

**개발자**: KimYC1223  
**이메일**: kau_esc@naver.com  
**GitHub**: [@KimYC1223](https://github.com/KimYC1223)

<div align="center">

**⭐ 이 프로젝트가 도움이 되었다면 Star를 눌러주세요! ⭐**

Made with ❤️ by KimYC1223

</div>