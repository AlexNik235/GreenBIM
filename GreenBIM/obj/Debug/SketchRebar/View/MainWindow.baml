   M S B A M L   `   `   `  ����C  ?GreenBIM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:GreenBIM.SketchRebar.ViewGreenBIM.SketchRebar.View  5      Q-clr-namespace:GreenBIM.SketchRebar.ViewModelsGreenBIM.SketchRebar.ViewModels  5      *    $GreenBIM.SketchRebar.View.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:GreenBIM.SketchRebar.View     5      <vm-clr-namespace:GreenBIM.SketchRebar.ViewModels     5      -   5        $��Window��5	        � SizeToContent$  WidthAndHeight=�5
       � 	Resources 5       �� 
TargetType   StandartButton �� Property �� Value��%�   &	        �� 5   
   " ��6   .�5      ��  � ��$ 25��6'   �� 5       � ��$ 80��6&   �� 5       � ��$ 10q�6'   5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5       � RowDefinitions 5   
   �� 5      �� 5      �� 5      .+�5   
   �� $��2��5      $��1��5      ��5       �� *UserSettingsList+��5!      �� 5"      .R�5#      ��  �� Orientation$ 
Horizontal=�6!   .+�5$      �� ��6$   �� *NumberOfColumn+�� 5%      $��30��6?   ��6"   �� *NumberOfRow+6?   5&      5'      5)      �� 5+   
   #��[  5,      $��2��5-      $��0��5.      
�������
����  �$��Принять��51      ��52      �� *FirstWinApplyCommand+��53      ��  �� ElementName$ Window���� 55   
   #��[  56      $��2��57      $��2��58      
���  ��
�������$��Отменить��5;      ��5<      �� *CancelCommand+��5=      �� $ Window��5@      5A      