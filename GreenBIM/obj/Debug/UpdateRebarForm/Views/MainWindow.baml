   M S B A M L   `   `   `  ����C  ?GreenBIM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:GreenBIM.UpdateRebarForm.ViewsGreenBIM.UpdateRebarForm.Views  5      Y1clr-namespace:GreenBIM.UpdateRebarForm.ViewModels#GreenBIM.UpdateRebarForm.ViewModels  5      /    )GreenBIM.UpdateRebarForm.Views.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >local,clr-namespace:GreenBIM.UpdateRebarForm.Views     5      @vm1clr-namespace:GreenBIM.UpdateRebarForm.ViewModels     5      -   5        $��Window��5
   =     � Title$H  BРабота с формами арматурых стержней��5      $	��180��5
      $	��380��6    � 
ResizeMode$ NoResize=�6'    � 	Resources 5       �� 
TargetType   Standart �� Property �� Value  StandartRadioButtn��%  &	        &	 �     �� 5   
   " ��6   .�5      ��  � ��$ 25��6'   �� 5       � ��$ 80��6&   �� 5       � ��$ 16(�6)   5      �� 5   
   " 	�6   .�5      ��  � ��$ 16(�6)   �� 5       � ��$ 3q�6'   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��85 �6   �� 5      $��40 �6   .+�5   
   ��  �� Orientation$ Vertical=�6   $��5q�6,   .+�5      	� $P��JИзбавиться от дубликатов системных форм��5      #��[ 5       d� 	IsChecked 5      �� *userSettings.ChangeRebarForm+	� 5       $#��Раскрыть группы��5!      #��[ 5"       5#      �� *userSettings.OpenGruops+	� 5$      $!��Склеить группы��5%      #��[ 5&       5'      �� *userSettings.CloseGruops+5(      �� 5*      $��1��5+      $��Принять��5,      #��[  5-      
����  �
�������$
��Left=�50      $��Center=�51      $��20,9,0,2q�52      ��53      �� *ApplyCommand+��54      �� 	 �� ElementName$	 Window���� 55   
   $��1��56      $��Отмена��57      #��[  58      
���  ��
�������$��Right=�5;      $��Center=�5<      $��0,9,20,2q�5=      ��5>      �� *CancelCommand+��5?      �� $	 Window��5@      5A      