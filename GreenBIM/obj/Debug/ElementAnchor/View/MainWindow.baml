   M S B A M L   `   `   `  ����C  ?GreenBIM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:GreenBIM.ElementAnchor.ViewGreenBIM.ElementAnchor.View  5      ,    &GreenBIM.ElementAnchor.View.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:GreenBIM.ElementAnchor.View     5      -   5        $��Window��5        � 
ResizeMode$  NoResize=�6-   $	��220��5   7   $	��300��6D   .��5
      �  � ColumnDefinitions 5   
   �� 5      $	��300 �6    � RowDefinitions 5   
   �� 5      $	��200 �6   .+�5   
   �� $��Принять��6   $
��Left=�6#   $��
33,139,0,0q�6>   $	��Top=�6R   $��75��6j   ��5      ��  �� Path$ ApplyCommand���67   ��  �� ElementName$ Window���� 5   
   $��Отмена��6   $
��Left=�6"   $��185,139,0,0q�6=   $	��Top=�6R   $��75��6j   ��5      �� $ CancelCommand���68   �� $ Window���� 5   
   -   5         ��Name$ 
stackPanel��5      $��26,41,39,94q�6)   .+�5      	� $@��:Закрепить все элементы в модели��6   $��0,10,0,0q�6e    d� 	IsChecked 6D   �� $ _Var1�6e   	� 5      $@��:Закрепить все элементы на листе��6   $��0,10,0,0q�6e    6D   �� $ _Var2�6e   5      �� 5   
   $3��-Выберите один из пунктов��6   $��Center=�63   $��Center=�6P   $��68,0,76,174q�6k   5      5      