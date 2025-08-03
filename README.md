<a name="english"></a>

# DampingSystem

<div align="center">

![Unity](https://img.shields.io/badge/Unity-2022.3.20f1+-000000.svg?style=flat&logo=unity&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-blue.svg?style=flat)

**Second-Order Damping Animation Library for Unity**

Youtube video [Giving Personality to Procedural Animations using Math](https://www.youtube.com/watch?v=KPoeNZZ6H4s) inspired Unity package.

하단에 [한국어 번역](#korean)이 있습니다.

</div>

---

## 📋 Table of Contents

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
- 🎯 **Multiple Type Support**: Full support for float, Vector2/3/4
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

The DampingSystem package provides specialized implementations for different data types, each optimized for their specific use cases:

### Scalar Type

```csharp
// Float damping - perfect for UI animations, health bars, etc.
var floatDamper = new DampingSystemFloat(2.0f, 1.0f, 0.0f, 0.0f);
float smoothValue = floatDamper.Calculate(targetValue);
```

### Vector Types

```csharp
// 2D position damping - ideal for 2D games and UI elements
var vec2Damper = new DampingSystemVector2(1.5f, 0.8f, 0.0f, Vector2.zero);

// 3D position damping - for smooth character/camera movement
var vec3Damper = new DampingSystemVector3(2.0f, 1.0f, 0.1f, Vector3.zero);

// 4D vector damping - useful for color transitions, RGBA values
var vec4Damper = new DampingSystemVector4(3.0f, 1.2f, 0.0f, Vector4.one);
```


### Implementation Structure

All damping systems inherit from the abstract base class `DampingSystem<T>` located in:
```
Assets/DampingSystem/Scripts/Abstract/DampingSystem.cs
```

Each type-specific implementation can be found in:
- `DampingSystemFloat.cs`
- `DampingSystemVector2.cs` 
- `DampingSystemVector3.cs`
- `DampingSystemVector4.cs`

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
| Vector2         | ~30                 | 80 bytes     |
| Vector3         | ~45                 | 96 bytes     |
| Vector4         | ~60                 | 112 bytes    |

---

## 🎯 Comparison with Other Libraries

### 🔷 DOTween

DampingSystem is a Unity library that implements natural damping animation through second-order differential equation-based physics simulation. It is controlled by physics parameters like frequency and dampingRatio, and its struct-based design allows for zero memory allocation and high performance (15-60 ns). DOTween, on the other hand, is an interpolation-based tweening library that provides precise timing control and a variety of animation effects through duration and easing functions.

The key difference lies in the approach. DampingSystem simulates a physically accurate spring-damper system, providing natural responses even when target values change in real-time, making it ideal for camera tracking and character movement. DOTween animates along predefined paths and times, making it ideal for situations requiring precise timing, such as UI animations and cutscenes. While DampingSystem is approximately 2-4 times faster and more memory-efficient, DOTween offers a richer feature set and a more robust sequencing system.

#### 📊 Differences

| Items | DampingSystem | DOTween |
|:---:|:---|:---|
| **Mathematical Basis** | Second-Order Differential Equation-Based Physics Simulation | Interpolation-Based Animation |
| **Dependencies** | Uses only Unity built-in functions | External Libraries |
| **Memory Allocation** | Zero-Allocation (struct-based) | Requires Object Pooling |
| **Performance** | 15-60ns (depending on type) | 100-200ns |
| **Animation Style** | Physically Natural Movements | Various Easing Functions |
| **Control Method** | Physics Parameters (frequency, damping) | Time-Based (duration, delay) |

#### When to Choose DampingSystem

- ✅ When you need **natural physical movement**
- Camera tracking, character movement, smooth UI element response
- ✅ When you need **real-time responsive animation**
- When target values constantly change (mouse tracking, player following)
- ✅ When you want to **minimize external dependencies**
- ✅ When you want to **control physical properties** (damping ratio, frequency, etc.)

#### When to Choose DOTween

- ✅ When you need **precise timing control**
- Cutscenes, sequence animations, and timing-critical productions
- ✅ When you need **various easing effects**
- Special effects such as Ease In/Out, Bounce, and Elastic
- ✅ When you need **complex animation chains**
- Sequential animation, callbacks, and event systems
- ✅ When you need specialized features for **UI animation**
- Various property animations such as size, rotation, color change, etc.

---

## 🔗 References

* [Giving Personality to Procedural Animations using Math](https://www.youtube.com/watch?v=KPoeNZZ6H4s)

---

## 📞 Contact

**Developer**: KimYC1223  
**Email**: kau_esc@naver.com  
**GitHub**: [@KimYC1223](https://github.com/KimYC1223)

<div align="center">

**⭐ If this project was helpful, please give it a star! ⭐**

Made with ❤️ by KimYC1223

</div>

<br />

---

<br />

<a name="korean"></a>

# DampingSystem

<div align="center">

![Unity](https://img.shields.io/badge/Unity-2022.3.20f1+-000000.svg?style=flat&logo=unity&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-blue.svg?style=flat)

**Unity용 2차 시스템 기반 감쇠 애니메이션 라이브러리**

Youtube 영상 [Giving Personality to Procedural Animations using Math](https://www.youtube.com/watch?v=KPoeNZZ6H4s)을 보고 영감을 받아 만든 Unity package입니다.

There is a [English translation](#english) at the top.

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
- 🎯 **다양한 타입 지원**: float, Vector2/3/4 완벽 지원
- 🔧 **사용 편의성**: 직관적인 매개변수로 쉬운 설정
- 📚 **완전한 문서화**: 한국어/영어 이중 문서화

---

## 🔧 설치 방법

[최신의 release된 Unity Package](https://github.com/KimYC1223/DampingSystem/releases)를 다운로드 하거나, 이 repo를 다운받으면 됩니다.

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

DampingSystem 패키지는 각각의 특정 사용 사례에 최적화된 다양한 데이터 타입별 특화 구현을 제공합니다:

### 스칼라 타입

```csharp
// Float 감쇠 - UI 애니메이션, 체력바 등에 완벽
var floatDamper = new DampingSystemFloat(2.0f, 1.0f, 0.0f, 0.0f);
float smoothValue = floatDamper.Calculate(targetValue);
```

### 벡터 타입

```csharp
// 2D 위치 감쇠 - 2D 게임과 UI 요소에 이상적
var vec2Damper = new DampingSystemVector2(1.5f, 0.8f, 0.0f, Vector2.zero);

// 3D 위치 감쇠 - 부드러운 캐릭터/카메라 움직임용
var vec3Damper = new DampingSystemVector3(2.0f, 1.0f, 0.1f, Vector3.zero);

// 4D 벡터 감쇠 - 색상 전환, RGBA 값에 유용
var vec4Damper = new DampingSystemVector4(3.0f, 1.2f, 0.0f, Vector4.one);
```


### 구현 구조

모든 감쇠 시스템은 다음 위치의 추상 기본 클래스 `DampingSystem<T>`를 상속받습니다:
```
Assets/DampingSystem/Scripts/Abstract/DampingSystem.cs
```

각 타입별 구현은 다음에서 찾을 수 있습니다:
- `DampingSystemFloat.cs`
- `DampingSystemVector2.cs` 
- `DampingSystemVector3.cs`
- `DampingSystemVector4.cs`

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
| Vector2         | ~30           | 80 bytes    |
| Vector3         | ~45           | 96 bytes    |
| Vector4         | ~60           | 112 bytes   |

---

## 🎯 다른 라이브러리와 비교

### 🔷 DOTween

DampingSystem은 2차 미분방정식 기반의 물리 시뮬레이션을 통해 자연스러운 감쇠 애니메이션을 구현하는 Unity 라이브러리입니다. frequency와 dampingRatio 같은 물리 매개변수로 제어하며, struct 기반 설계로 제로 메모리 할당과 15-60ns의 고성능을 제공합니다. 반면 DOTween은 보간 기반의 트위닝 라이브러리로, duration과 easing 함수를 통해 정확한 시간 제어와 다양한 애니메이션 효과를 제공합니다.

핵심 차이점은 접근 방식에 있습니다. DampingSystem은 물리적으로 정확한 스프링-댐퍼 시스템을 시뮬레이션하여 목표값이 실시간으로 변하는 상황에서도 자연스러운 반응을 보이며, 카메라 추적이나 캐릭터 이동에 적합합니다. DOTween은 미리 정의된 경로와 시간을 따라 애니메이션하므로 UI 연출이나 컷신 같은 정확한 타이밍이 필요한 상황에 최적화되어 있습니다. 성능 면에서 DampingSystem이 약 2-4배 빠르고 메모리 효율적이지만, DOTween은 더 풍부한 기능과 시퀀싱 시스템을 제공합니다.

### 📊 핵심 차이점

| 항목 | DampingSystem | DOTween |
|:---:|:---|:---|
| **수학적 기반** | 2차 미분방정식 기반 물리 시뮬레이션 | 보간 기반 애니메이션 |
| **의존성** | Unity 내장 기능만 사용 | 외부 라이브러리 |
| **메모리 할당** | 제로 할당 (struct 기반) | 객체 풀링 필요 |
| **성능** | 15-60ns (타입별) | 100-200ns |
| **애니메이션 스타일** | 물리적으로 자연스러운 움직임 | 다양한 이징 함수 |
| **제어 방식** | 물리 매개변수 (frequency, damping) | 시간 기반 (duration, delay) |

### DampingSystem을 선택해야 하는 경우

- ✅ **자연스러운 물리적 움직임**이 필요한 경우
  - 카메라 추적, 캐릭터 이동, UI 요소의 부드러운 반응
- ✅ **실시간 반응형 애니메이션**이 필요한 경우
  - 목표값이 계속 변하는 상황 (마우스 추적, 플레이어 따라가기)
- ✅ **외부 의존성을 최소화**하고 싶은 경우
- ✅ **물리적 특성을 조절**하고 싶은 경우 (감쇠비, 진동수 등)

### DOTween을 선택해야 하는 경우

- ✅ **정확한 시간 제어**가 필요한 경우
  - 컷신, 시퀀스 애니메이션, 타이밍이 중요한 연출
- ✅ **다양한 이징 효과**가 필요한 경우
  - Ease In/Out, Bounce, Elastic 등의 특수 효과
- ✅ **복잡한 애니메이션 체인**이 필요한 경우
  - 순차적 애니메이션, 콜백, 이벤트 시스템
- ✅ **UI 애니메이션**에 특화된 기능이 필요한 경우
  - 크기, 회전, 색상 변경 등의 다양한 프로퍼티 애니메이션
  
---

## 🔗 참고자료

* [Giving Personality to Procedural Animations using Math](https://www.youtube.com/watch?v=KPoeNZZ6H4s)

---

## 📞 연락처

**개발자**: KimYC1223  
**이메일**: kau_esc@naver.com  
**GitHub**: [@KimYC1223](https://github.com/KimYC1223)

<div align="center">

**⭐ 이 프로젝트가 도움이 되었다면 Star를 눌러주세요! ⭐**

Made with ❤️ by KimYC1223

</div>