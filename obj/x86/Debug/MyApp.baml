   M S B A M L   `   `   `  ����T  PDiscreteGestureBasics-WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullo<clr-namespace:Microsoft.Samples.Kinect.DiscreteGestureBasics.Microsoft.Samples.Kinect.DiscreteGestureBasics  5   	   :    4Microsoft.Samples.Kinect.DiscreteGestureBasics.MyApp   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Nlocal<clr-namespace:Microsoft.Samples.Kinect.DiscreteGestureBasics     5   	     � Title$  MyApp��5   	   $	��300��6   $	��300��6$   .��5	      � .+�5
   
   �� -   5         ��Name$ username_txtbx��5
      $
��Left=�6*   $��23��6E   $��	81,58,0,0q�6Q    �� TextWrapping$
 Wrap=�6d   $	��Top=�6x   $	��120��6�   $��Username��6�   �� 5   
   -   5         ��Name$ register_btn��5      $��Register��6'   $
��Left=�6:   $��
81,123,0,0q�6U   $	��Top=�6i   $��75��6�   6�   �� 5   
   -   5        $ 
signIn_btn��5      $��Already Registered��6%   $
��Left=�6B   $��
81,225,0,0q�6]   $	��Top=�6q   $	��120��6�   6�   �� 5   
   -   5         ��Name$ register_label��5      $��Register/Train��6(   $
��Left=�6A   $��	81,22,0,0q�6\   $	��Top=�6o   $��16(�6�   $
��Bold"�6�   �� 5   
   -   5        $ enter_label��5      $��Enter Gesture��6%   $
��Left=�6=   $��
84,103,0,0q�6X   $	��Top=�6l   $��18(�6�   $
��Bold"�6�   �� 5   
   -   5        $ notRegistered_btn��5      $��Not Registered��6,   $
��Left=�6E   $��
81,206,0,0q�6`   $	��Top=�6t   $	��120��6�   6�   �� 5   
   -   5        $ 	start_btn��5      $��Start��6$   $
��Left=�64   $��107,162,0,0q�6O   $	��Top=�6d   $��75��6|   6�   �� 5   
   -   5        $ signInStart_btn��5      $��Sign In��6*   $
��Left=�6<   $��107,183,0,0q�6W   $	��Top=�6l   $��75��6�    J� RenderTransformOrigin$ -0.156,0.586&�6�   $y�Hidden=�6�   �� 5   
   -	   5         ��Name$ gestureComboBox��5      $
��Left=�6,   $��	81,86,0,0q�6G   $	��Top=�6Z   $��74��6r   6}   �� 5   
   -
   5        $ 	train_btn��5      $��Train��6$   $
��Left=�64   $��
81,142,0,0q�6O   $	��Top=�6c   $��75��6{   6�   5      5      