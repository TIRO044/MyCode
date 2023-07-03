# **Unity_UGUIFrameWork**

UI 뿐만 아니라, 아웃 게임 프레임워크를 제작 중 입니다. (좋은 생각이 들었을 때 심심풀이 겸)

핵심 키워드 간단 소개.

## IInstanceManager
 - 추상화된 인스턴스 매니저 입니다.
 - 기본적으로, 팩토리, 풀링이 구현되도록 작업했습니다.

## MVVM
 ### :minidisc: View
 - 유니티 인스펙터에서 ViewModel의 타입을 선택합니다.
 - 그럼 런타임 View가 초기화 되는 타이밍에 ViewModel인스턴스를 View에 만들고, 프로퍼티들을 자동으로 바인딩 해줍니다.
 - ViewModel을 할당할 때, 의존성을 낮추기 위해 DI(VContainer)를 적용했습니다.
 ### :minidisc: ViewModel
 - View에 프로퍼티들이 바인딩되며, 프로퍼티의 값이 변할 때 View에 알려줍니다.
 ### :minidisc: ViewApplier
 - View와 ViewModel의 의존성을 제거하기 위해 도입한 개념입니다.
 - VContainer로 할당된 ViewModel의 프로퍼티가 변할 때, View에서 동작할 동작을 처리합니다.
 ### :cd: Model (미구현)
 - Model은 비즈니스 로직과 관련이 깊어서, 좀 더 코어한 부분작업할 때 추가할 예정
