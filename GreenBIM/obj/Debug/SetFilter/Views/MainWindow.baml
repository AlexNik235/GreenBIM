   M S B A M L   `   `   `  ����C  ?GreenBIM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:GreenBIM.SetFilter.ViewsGreenBIM.SetFilter.Views  5      K*clr-namespace:GreenBIM.SetFilter.ViewModelGreenBIM.SetFilter.ViewModel  5      )    #GreenBIM.SetFilter.Views.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:GreenBIM.SetFilter.Views     5      9vm*clr-namespace:GreenBIM.SetFilter.ViewModel     5      -   5        $��Window��5	        � Topmost  . $	��550��5
      $	��330��6   .��5      � $
��Left=�6   $	��315��6&    � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $��* �6   �� 5      $	��110 �6    � ColumnDefinitions 5   
   �� 5      $
��auto �6   .+�5   
   �� -   5        $��	10,5,41,5q�5      
���   �$��2q�5       �� CornerRadius$ 2��5      
�������._�5      �� $w��qТекущие фильтры                         (кликнуть мышкой для обновления)��5      $
��Left=�5      $
��Wrap=�5      $��0��5       �� TextAlignment$ Center=�5        � ToolTip$� �Плагин не умеет обновлять фильтры при перехода с одного вида на другой автоматически. Поэтому требуется кликнуть мышью по надписи��5!      $��Center=�5"       ^� 	Placement$ Mouse=�5#      $��31,1,0,3q�62   $	��226��6D   5%      �� 5'   
   $��1��6   $��
10,10,0,10q�5(      $
��Left=�5)   c   $	��293��6~   ��6   ��  �� Path$ CurFinlterList� �� UpdateSourceTrigger$ PropertyChanged=���5*      �� 5+      .R�5,      �� 	 �� Orientation$	 
Horizontal=�6!   .+�5-      �� $��0��50      $! Выбор элемента��51      $ Mouse=�52      ��5.      �� *
FilterName+
 d� 	IsChecked
 5/      �� *IsOn+52      53      54      56      �� 58   
   $	 Vertical=�59      $��0��5:      $��2��5;      $��10,23,36,45q�6   .+�5<      �� $��14(�5=      $A��;Включить шаблон временного вида��5>      
 5?      �� $# _userSetting.SetTemporaryView��� 5A      $��14(�5B      $6��0Удалить выбранные фильтры��5C      
 5D      �� *_userSetting.DelitElement+5F      �� 5H   
   $��2��6   $��30��5I      $��60��5J      ���#0078D4
�������$��Принять��5N   A   $��2q�6S   $��10,70,274,10q�6g   ��5M      �� $ MainWindowApplyCommand���5N      ��  �� ElementName$ Window��6g   �� 5O   
   $��2��6   $��30��5P      $��60��5Q      $��Отмена��5T      
���  ��
�������$��2q�5W      $��243,71,41,9q�5X      ��5R      �� $ Window����5S      �� $ CancelCommand�5X      �� 5Y   
   $��2��6   $��30��5Z      
�������$%��Добавить фильтры��5^      $��2q�5_      $��78,71,110,9q�5`      ��5\      �� $ Window����5]      �� $ ShowFirstWinCommand�5`      5b      5c      