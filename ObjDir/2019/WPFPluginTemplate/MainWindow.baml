   M S B A M L   `   `   `  ����L  HWPFPluginTemplate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:WPFPluginTemplateWPFPluginTemplate  5   	   a ]Tekla.Structures.Dialog, Version=2020.0.0.0, Culture=neutral, PublicKeyToken=2f04dbe497b71114bFclr-namespace:Tekla.Structures.Dialog;assembly=Tekla.Structures.DialogTekla.Structures.Dialog 5   	   xQclr-namespace:Tekla.Structures.Dialog.UIControls;assembly=Tekla.Structures.Dialog"Tekla.Structures.Dialog.UIControls 5   	   "    WPFPluginTemplate.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:WPFPluginTemplate     5   	   VtsdFclr-namespace:Tekla.Structures.Dialog;assembly=Tekla.Structures.Dialog     5   	   h
UIControlsQclr-namespace:Tekla.Structures.Dialog.UIControls;assembly=Tekla.Structures.Dialog     5   	   $	��450��5
   -   $	��600��6:   $��16(�6F     � Title  6	   *  $Tekla.Structures.Dialog.LocExtension  *albl_Title_Plugin+ � DataContext 5      +   %WPFPluginTemplate.MainWindowViewModel  5   
   6&   .��5      �  � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      �� 5      �� 5      $��50 �6   .+�5   
   �� $��1��6   $	��200��6   $	��200��6*   $
��Left=�67   $��	0,-30,0,0q�6R   .+�5      �� $	��120��6   $	��120��6$   $��40q�61   
����  �$��4��6K    �� RadiusX$ 5��6_    �� RadiusY$ 5��6k   6y   A� 5      
���  ��$��30��6#   $��30��6.   
���  ��$��50q�6E   6S   A� 5      
���  ��$��30��6#   $��30��6.   
���  �� �� Left$	 114��6E   
 �� Top$ 50��6W   A� 5      
���  ��$��30��6#   $��30��6.   
���  ��$ 50��6E   $	 115��6V   A� 5      
���  ��$��30��6#   $��30��6.   
���  ��$	 114��6E   $	 115��6W   5      4  .Tekla.Structures.Dialog.UIControls.WpfSaveLoad  5   
   $	��582��6"   $��50��6.   $��Center=�6:   $��Center=�6W   G  ATekla.Structures.Dialog.UIControls.WpfOkApplyModifyGetOnOffCancel  5   
   -   5        $��Stretch=�5   4   $��3��6S   $��Stretch=�6`   6�  �� 5   
   $��
Image1.png��6   $��1��6%   $��182,10.189,0,10.208q�62   $
��Left=�6O   $��UniformToFill=�6j   $	��321��6�   � 5    
   $��2��6    � 	Resources 5!       �� 
TargetType	 �� Property
 �� Value:  4Tekla.Structures.Dialog.UIControls.WpfFilterCheckBox��%2  '��       '�� M     '  �     �� 5"      " ��6   .�5#      �� 	 � ��$
 30��6/   6<   5$      �� 5%      " ��6   .�5&      �� 	 � ��$
 30��6/   6<   5'      �� 5(      "  6   .�5)      �� 	 � ��$
 30��6/   6<   �� 5*      	 � ��$
 Center=�6A   6R   5+      5,      .+�5-      �� $
��Left=�6   $��10,0,0,0q�64   .+�5.      ��  �� Orientation$ 
Horizontal=�6   .+�5/           AttributeName$ 	RebarStep��63   �� 50      $��Шаг хомутов��6   $	��150��62   6@   �� 51      $	��150��69   ��6   �� *	RebarStep+6G   52      �� 53      $ 
Horizontal=�6   .+�54        $ 	RebarSize��63   �� 55      $��Ø хомутов��6   $	��150��60   6>   �� 56      $	��150��69   ��6   �� *	RebarSize+6G   57      �� 58      $ 
Horizontal=�6   .+�59        $ Profile��64   �� 5:      $��Профиль��6   $	��150��6.   6<   �� 5;      $	��150��67   ��6   �� *	Profile+6E   5<      �� 5=      $ 
Horizontal=�6   .+�5>        $ Material��64   �� 5?      $��Материал��6   $	��150��6/   6=   �� 5@      $	��150��68   ��6   �� *
Material+6F   ;  5Tekla.Structures.Dialog.UIControls.WpfMaterialCatalog  5A      -   5          Name$ MaterialCatalog��5A   4   $��Center=�6�   $��Center=�6�   5B      5C      5E      5F      5G      