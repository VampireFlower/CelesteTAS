#!/usr/bin/env python
# This script converts .tas files from Celeste TAS tool
# (https://github.com/ShootMe/CelesteTAS/) to libTAS input file.
# Just run ./Celeste2libTAS path/to/tasfile.tas

import glob
import math
import os
import re
import sys


def main():
    Celeste2libTAS().convert()


def get_line(label_or_line_number, file):
    try:
        return int(label_or_line_number)
    except ValueError:
        current_line = 0

        for line in file:
            current_line += 1
            if line == f'#{label_or_line_number}\n':
                return current_line

        return float('inf')


class Celeste2libTAS:
    def __init__(self):
        self.input_file = None
        self.output_file = None
        self.regex_input = re.compile(r'[\s]*([\d]*)((?:,(?:[RLUDJKXCGSQNFO]|[\d.]*))*)')
        self.regex_comment = re.compile(r'[\s]*(#|[\s]*$)')
        self.frame_counter = 0

    def convert(self):
        self.input_file = open(sys.argv[1], 'r')
        self.output_file = open(f'{os.path.splitext(sys.argv[1])[0]}.ltm', 'w')

        # Perform the actual conversion
        self.export_file(self.input_file)

        self.output_file.close()

    def get_read_data(self, line: str):
        index = line.find(',')
        if index > 0:
            file_name = line[0:index]
        else:
            file_name = line[0:-1]

        file_path = f'{os.path.dirname(sys.argv[1])}/{file_name}.tas'
        # Check if full filename was used, get file if it wasn't
        if not os.path.exists(file_path):
            file_prefix = f'{os.path.dirname(sys.argv[1])}/{file_name}'
            files = [f for f in glob.glob(f'{file_prefix}*.tas')]
            if not files:
                return None, None, None
            file_path = str(files[0])

        file = open(file_path, 'r')
        skip_lines = 0
        line_len = float('inf')

        # Check how many line numbers were given and convert any labels to lines
        if index > 0:
            index_len = line.find(',', index + 1)
            if index_len > 0:
                start_line = line[index + 1: index_len]
                end_line = line[index_len + 1:-1]
                skip_lines = get_line(start_line, file)
                line_len = skip_lines + get_line(end_line, file)
            else:
                start_line = line[index + 1:-1]
                skip_lines = get_line(start_line, file)

        if skip_lines is None:
            skip_lines = 0

        print(f"Reading {line[0:-1]} from {skip_lines} to {line_len}, at frame {self.frame_counter}")
        return file, skip_lines, line_len

    def export_file(self, file, start_line=0, end_line=float('inf')):
        file.seek(0)
        cur_line = 0
        skip_line = False

        for line in file:
            cur_line += 1
            line_lower = line.lower()

            if cur_line <= start_line:
                continue
            if cur_line > end_line:
                break
            if skip_line:
                skip_line = False
                continue
            if self.regex_comment.match(line):
                continue
            if line_lower.startswith('read'):
                read_path, start, end = self.get_read_data(line[5:])
                if read_path is not None:
                    self.export_file(read_path, start, end)
                continue
            if line_lower.startswith('add'):
                line = line[3:]
            if line_lower.startswith('skip'):
                skip_line = True
                continue

            match = self.regex_input.match(line)
            if match:
                output_keys = ''
                button_order = 'ABXYbgs()[]udlr'
                button_mapping = 'JXCK..S...GUDLR'
                output_buttons = ['.'] * 15
                output_axes = '0:0'
                is_axis = False

                for single_input in match.group(2).split(',')[1:]:
                    if is_axis:
                        angle = 0 if single_input == '' else float(single_input)

                        # Compute coordinates of the left analog stick to match the
                        # requested angle. Use the max amplitude to get precise values.
                        # We must also compensate for the deadzone which is 0.239532471f
                        rad_angle = math.radians(angle)
                        deadzone = 0.239532471
                        float_x = math.copysign(math.fabs(math.sin(rad_angle)) * (1 - deadzone) + deadzone, math.sin(rad_angle))
                        float_y = math.copysign(math.fabs(math.cos(rad_angle)) * (1 - deadzone) + deadzone, math.cos(rad_angle))

                        x = 32767 * float_x
                        y = -32767 * float_y
                        output_axes = f'{str(int(x))}:{str(int(y))}'

                        is_axis = False
                        continue

                    if single_input == 'F':
                        is_axis = True
                        continue

                    if single_input == 'O':
                        output_keys = 'ff0d'
                    elif single_input == 'Q':
                        output_keys = '72'
                    else:
                        output_keys = ''

                    # Look at the mapping of the action
                    mapped_index = button_mapping.find(single_input)
                    output_buttons[mapped_index] = button_order[mapped_index]

                # Write the constructed input line, ignore false positive matches
                output_line = f'|{output_keys}|{output_axes}:0:0:0:0:{"".join(output_buttons)}|.........|\n'
                try:
                    for n in range(int(match.group(1))):
                        self.frame_counter += 1
                        self.output_file.write(output_line)
                except ValueError:
                    print(f"Ignoring {line[0:-1]}")

        print(f"Read {cur_line - start_line} lines from {file.name}")
        file.close()


if __name__ == '__main__':
    main()
