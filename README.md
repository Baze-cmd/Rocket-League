Rocket League
<br/>
School project by: Благојче Димов
<br/>
#1 Што е апликацијата?
<br/>
Како проект избрав да направам дводимензионална верзија на Rocket League која е игра базирана на физика(линк до пример гејмплеј: https://www.youtube.com/watch?v=s68V-K6H1mE), оваа игра е многу едноставна за сваќање но многу тешка за совладување. Играта се состој од една топка и една кола(може и повеќе) на мапа, преку управување на колата целта е топката да влезе во голот. Колата може да има флип(flip) со што ќе се забрза и ротира кон насоката на џојстикот, ако колата е нацртана со полн правоаголник тогаш има флип, исто така има и ограничен буст(boost) максимум 100 и се регенерира единствено кога колата е во допир со страните на мапата под одреден агол и се гледа на горниот лев дел од сликата. Со управување на џојстикот може колата да се ротира во истата насока. На прво отварање на програмата се стартува почетно мени, засега нема многу опции освен за да се влезе во игра, исто така има и мал број поставувања како опција да се исклучи музика и најбитното од се "high fps mode", препорачано е ова да се вклучи.
<br/>
#2 Како е имплементирана?
<br/>
Во целиот проект постојат 6 C# класи: Vector, Button, Ball, Car, World, Rocket League. Vector класата е едноставна вектор класа за операции со вектори, Button класата содржи информации за копчињата кој се гледаат на почетното стартување и дополнителни поставки. Нај битните класи се Ball и Car,топката е представена со позиција и радиус,додека пак колата е малку по коплицирано затоа во целoст се чуваат 5 точки за нејзино опишување една како центар на маса точка и 4 други кој ги опишуваат 4те краеви на колата, за ротирање на колата се користи функција во Vector класата која се ротира себеси околу дадена точка ceter под одреден агол void RotateAround(Vector2D center, double angle), во овие класи исто така се црта и пресметува дали постој колизија на објектот со страните на мапата во секој фрејм и ако има на кој начин ќе се реши. Затоа што играта е базирана на физика не треба да постојат ненадејни промени во позицијата на овие објекти. Во двете класи има функции за успорување на брзината во спротивно ќе има целосно еластични удари и за пример топката никогаш нема да слета на земјата. Додека пак во класата World се чува информации за самата мапа колку е широка и висока, меѓутоа и тука се прави пресметка дали има колизија помеѓу самата кола и топка и на кој начин топката ќе реагира од тој удар, засега резолуцијата не е одлична но it's not a bug it's a feature :|. Една од нај интересните работи е како топката удира на крајевите на мапата. Овдека имав за цел краевите да бидат заоблени, за таа цел се користи низа од линии во секој ќош што би симулирал крива. И колизијата на топката со низите од линии е на таков начин што импулсот на топката e совршено пресликани во соодветната насока како што за пример аголот на светлина која удра во огледало останува ист, за оваа цел е употребена формуата за рефлекција на вектор кој се користи во raytracing. Истата тактика е користена и за рефлекција на топката со колата.
<br/>
#3 Како се игрa?
<br/>
Во главното мени се притиска на копчето "freeplay" и со управување на џојстикот и копчињата "a" boost и "d" за jump/flip
<br/>
(БИТНO: За подобро искуство овозможиете "high FPS mode" преку копчето "settings" или на тастатурата ESC)
<br/>
(кодот не е најдобар ни пак оптимизиран има многу делови кој што ќе треба да се исполираат и рефакторираат, дополнително во целиот проект има аудио фајл од тип wav и е одприлика 40МВ во големина 80% од целиот проект, mp3 верзијата е 4MB но незнам како во програма да прочитам mp3)
<br/>
Пример слики од главното мени и гејмплеј:
![A](https://github.com/Baze-cmd/Rocket-League/blob/master/main%20menu.png)
![A](https://github.com/Baze-cmd/Rocket-League/blob/master/ingame.png)
