   M S B A M L   `   `   `  ����N  JGirderSystemProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:GirderSystemProjectGirderSystemProject  5   	   $    GirderSystemProject.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:GirderSystemProject     5   	     � Title$  
MainWindow��5   	   $	��300��6   $	��400��6)    � 
ResizeMode$ CanMinimize=�65    � 	Resources 5	       �� 
TargetType �� Property �� Value��%�   '��       '�� M     �� 5
   
   " ��6   .�5      ��  � ��$ 30��6'   64   5      �� 5   
   " ��6   .�5      ��  � ��$ 30��6'   64   5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      $
��0.6* �6   6.   �� 5      6!    � RowDefinitions 5   
   �� 5      6   �� 5      $
��0.2* �6   6,   .+�5   
   �� .+�5      �� $��14(�6   .��6#   Ширина6+   �� 5      $��14(�6   .��6"    Профиль гл.балки64   �� 5      $��14(�6   .��6"    Профиль вт.балки64   �� 5      $��14(�6   .��6"   Шаг вт.балок60   �� 5      $��14(�6   .��6"   Материал6,   �� 5       $��14(�6   .��6"   !Номер соединения64   5"      �� 5#   
   $��1��6   .+�5$      �� ��6   �� *GirderWidth+65   �� 5%      ��6   �� *PrimaryProfile+68   �� 5&      ��6   �� *SecondaryProfile+6:   �� 5'      ��6   �� *Step+6.   �� 5(      ��6   �� *
Material+62   �� 5)      ��6   �� *ConnectionNumber+6:   5*      �� 5+   
   $��1��6   .+�5,      ��  d� 	IsChecked 6   �� *IsCreateDrawing+.��6=   Создать чертеж6M   5.      �� 5/   
   -   5        $��btn_CreateGirder��5/      $��1��6)   $��1��69   $	��170��6F   $��25��6R   $��12(�6^   .��6�   20Построить балочную клетку6�   50      51      